using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using StoryBlog.Infrastructure.AnyOf.Common;

namespace StoryBlog.Infrastructure.AnyOf.Tools;

internal sealed class AnyOfClassVisitor : CSharpSyntaxRewriter
{
    private static readonly Type GenerateAttribute = typeof(GenerateAnyOfAttribute);
    
    private readonly GeneratorExecutionContext context;

    public AnyOfClassVisitor(GeneratorExecutionContext context)
        : base(visitIntoStructuredTrivia: false)
    {
        this.context = context;
    }

    public override SyntaxNode? VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        if (ShouldExtendClass(node))
        {
            var typeParameters = GetGenericTypes(node);
            var generatedSyntax = SyntaxFactory
                .SyntaxTree(
                    root: GenerateClassImplementation(node, typeParameters).NormalizeWhitespace(),
                    path: GetFileName(node),
                    encoding: Encoding.UTF8
                );
            var hint = GetHint(node);
            var sourceText = CSharpCompilation.Create(null, new[] { generatedSyntax });
            var tempText = generatedSyntax.GetText().ToString();
            
            context.AddSource(hint, generatedSyntax.GetText());
        }

        return base.VisitClassDeclaration(node);
    }

    private static CompilationUnitSyntax GenerateClassImplementation(
        ClassDeclarationSyntax classDeclarationSyntax,
        TypeSyntax[] typeParameters)
    {
        var classDeclarationRoot = (CompilationUnitSyntax)classDeclarationSyntax.SyntaxTree.GetRoot();
        var classDeclaration = SyntaxFactory
            .CompilationUnit()
            .WithUsings(
                classDeclarationRoot.Usings
            )
            .WithMembers(
                SyntaxFactory.List(new MemberDeclarationSyntax[]
                {
                    SyntaxFactory
                        .ClassDeclaration(classDeclarationSyntax.Identifier)
                        .WithModifiers(classDeclarationSyntax.Modifiers)
                        .WithMembers(
                            GenerateConstructorImplementations(classDeclarationSyntax, typeParameters)
                        )
                })
            );
        //classDeclaration.Language
        //classDeclaration.SyntaxTree.W
        return classDeclaration;
    }

    private static SyntaxList<MemberDeclarationSyntax> GenerateConstructorImplementations(
        ClassDeclarationSyntax classDeclarationSyntax,
        TypeSyntax[] typeParameters)
    {
        const string identifierName = "value";

        var constructors = new MemberDeclarationSyntax[typeParameters.Length];

        for (int parameterIndex = 0; parameterIndex < typeParameters.Length; parameterIndex++)
        {
            constructors[parameterIndex] = SyntaxFactory
                .ConstructorDeclaration(classDeclarationSyntax.Identifier)
                .WithModifiers(
                    SyntaxFactory.TokenList(new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword)
                    })
                )
                .WithParameterList(
                    SyntaxFactory.ParameterList(
                        SyntaxFactory.SeparatedList(new[]
                        {
                            SyntaxFactory.Parameter(
                                SyntaxFactory.List<AttributeListSyntax>(),
                                new SyntaxTokenList(),
                                type: typeParameters[parameterIndex],
                                identifier: SyntaxFactory.Identifier(identifierName),
                                @default: null
                            )
                        })
                    )
                )
                .WithInitializer(
                    SyntaxFactory.ConstructorInitializer(
                        SyntaxKind.BaseConstructorInitializer,
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList(new[]
                            {
                                SyntaxFactory.Argument(
                                    SyntaxFactory.IdentifierName(identifierName)
                                )
                            })
                        )
                    )
                )
                .WithBody(
                    SyntaxFactory.Block()
                );
        }

        return SyntaxFactory.List(constructors);
    }

    private static string GetHint(ClassDeclarationSyntax classDeclarationSyntax)
    {
        var filePath = classDeclarationSyntax.SyntaxTree.FilePath;
        var fileName = Path.GetFileName(filePath);
        var extension = Path.GetExtension(fileName);

        fileName = Path.GetFileNameWithoutExtension(fileName);

        return fileName + ".generated" + extension;
    }

    private static string GetFileName(ClassDeclarationSyntax classDeclarationSyntax)
    {
        var filePath = classDeclarationSyntax.SyntaxTree.FilePath;
        var fileName = Path.GetFileName(filePath);
        var extension = Path.GetExtension(fileName);

        fileName = Path.GetFileNameWithoutExtension(fileName);
        fileName = fileName + ".generated" + extension;

        return Path.Combine(Path.GetDirectoryName(filePath), fileName);
    }

    private static TypeSyntax[] GetGenericTypes(ClassDeclarationSyntax classDeclarationSyntax)
    {
        var count = classDeclarationSyntax.BaseList?.Types.Count ?? 0;

        for (var baseTypeIndex = 0; baseTypeIndex < count; baseTypeIndex++)
        {
            var baseTypeSyntax = classDeclarationSyntax.BaseList.Types[baseTypeIndex];

            if (SyntaxKind.SimpleBaseType == baseTypeSyntax.Kind() && baseTypeSyntax is SimpleBaseTypeSyntax typeSyntax)
            {
                if (SyntaxKind.GenericName == typeSyntax.Type.Kind() && typeSyntax.Type is GenericNameSyntax genericSyntax)
                {
                    var temp = genericSyntax.TypeArgumentList.Arguments;
                    var types = new TypeSyntax[temp.Count];

                    for (var index = 0; index < temp.Count; index++)
                    {
                        types[index] = temp[index];
                    }

                    return types;
                }
            }
        }

        return Array.Empty<TypeSyntax>();
    }

    private static bool ShouldExtendClass(ClassDeclarationSyntax node)
    {
        const ExpectedModifiers expectedMask = ExpectedModifiers.Public | ExpectedModifiers.Partial;

        for (var attributeIndex = 0; attributeIndex < node.AttributeLists.Count; attributeIndex++)
        {
            var attributeSyntax = node.AttributeLists[attributeIndex];

            for (var index = 0; index < attributeSyntax.Attributes.Count; index++)
            {
                var syntax = attributeSyntax.Attributes[index];

                if (IsExpectedAttribute(syntax.Name))
                {
                    var modifiers = IsNotSealed(node.Modifiers);

                    if (expectedMask == (modifiers & expectedMask))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private static ExpectedModifiers IsNotSealed(SyntaxTokenList modifiers)
    {
        var expectedModifiers = ExpectedModifiers.None;

        for (var modifierIndex = 0; modifierIndex < modifiers.Count; modifierIndex++)
        {
            var modifier = modifiers[modifierIndex];

            if (false == modifier.IsKeyword())
            {
                continue;
            }

            switch (modifier.Kind())
            {
                case SyntaxKind.PublicKeyword:
                {
                    expectedModifiers |= ExpectedModifiers.Public;
                    break;
                }

                case SyntaxKind.PartialKeyword:
                {
                    expectedModifiers |= ExpectedModifiers.Partial;
                    break;
                }
            }
        }

        return expectedModifiers;
    }

    private static bool IsExpectedAttribute(NameSyntax nameSyntax)
    {
        switch (nameSyntax.Kind())
        {
            case SyntaxKind.QualifiedName:
            {
                var qualifiedNameSyntax = (QualifiedNameSyntax)nameSyntax;
                var namespaceName = qualifiedNameSyntax.Left.ToFullString();
                var position = GenerateAttribute.FullName?.LastIndexOf(namespaceName, StringComparison.InvariantCulture) ?? -1;

                if (0 > position)
                {
                    return false;
                }

                return IsExpectedAttributeName(qualifiedNameSyntax.Right.Identifier);
            }

            case SyntaxKind.IdentifierName:
            {
                var identifierNameSyntax = (IdentifierNameSyntax)nameSyntax;
                return IsExpectedAttributeName(identifierNameSyntax.Identifier);
            }
        }

        return false;
    }

    private static bool IsExpectedAttributeName(SyntaxToken syntaxToken) =>
        String.Equals(FixAttributeName(syntaxToken), GenerateAttribute.Name);

    private static string FixAttributeName(SyntaxToken syntaxToken)
    {
        const string attributeSuffix = "Attribute";
        var name = syntaxToken.Text;
        return name.EndsWith(attributeSuffix) ? name : name + attributeSuffix;
    }

    [Flags]
    private enum ExpectedModifiers : byte
    {
        None = 0x00,
        Public = 0x01,
        Partial = 0x02
    }

    private const string SampleText = "using System; using System.Debug; class Test1 { }";
}
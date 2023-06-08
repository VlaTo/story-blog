using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace StoryBlog.Infrastructure.AnyOf.Tools
{
    [Generator]
    public class AnyOfBootstrapCodeGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization(PostInitialize);
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var visitor = new AnyOfClassVisitor(context);

            foreach (var syntaxTree in context.Compilation.SyntaxTrees)
            {
                visitor.Visit(syntaxTree.GetRoot(context.CancellationToken));
            }
        }

        private void PostInitialize(GeneratorPostInitializationContext context)
        {
            //context.
        }
    }
}

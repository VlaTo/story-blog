using System.Text;
using MediatR;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Microservices.Posts.Application.Core;
using StoryBlog.Web.Microservices.Posts.Application.Services;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GenerateSlug;

public sealed class GenerateSlugHandler : HandlerBase, IRequestHandler<GenerateSlugQuery, Result<string>>
{
    private readonly IAsyncUnitOfWork context;
    private readonly IWordTransliterator transliterator;
    private readonly ILogger<GenerateSlugHandler> logger;

    public GenerateSlugHandler(
        IAsyncUnitOfWork context,
        IWordTransliterator transliterator,
        ILogger<GenerateSlugHandler> logger)
    {
        this.context = context;
        this.transliterator = transliterator;
        this.logger = logger;
    }

    public async Task<Result<string>> Handle(GenerateSlugQuery request, CancellationToken cancellationToken)
    {
        await using (var repository = context.GetRepository<Domain.Entities.Post>())
        {
            var tries = 3;

            while (0 < tries--)
            {
                var slug = await GenerateSlugAsync(request.Title);
                var exists = await repository.ExistsAsync(new FindSlugByTextSpecification(slug), cancellationToken);

                if (exists)
                {
                    continue;
                }

                return slug;
            }
        }

        return new Exception("Failed to generate slug");
    }

    private async ValueTask<string> GenerateSlugAsync(string text)
    {
        const char delimiter = '-';

        var builder = new StringBuilder();
        var reader = new WordReader(text);

        foreach (var word in reader.EnumerateWords())
        {
            var temp = await transliterator.TransliterateWordAsync(word);

            if (0 < builder.Length && delimiter != builder[^1])
            {
                builder.Append(delimiter);
            }

            builder.Append(temp ?? word);
        }

        return builder.ToString();
    }
}
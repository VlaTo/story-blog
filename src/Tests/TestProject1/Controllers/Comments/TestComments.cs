using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Microservices.Comments.Application.Contexts;
using StoryBlog.Web.Microservices.Comments.Application.Extensions;
using StoryBlog.Web.Microservices.Comments.Infrastructure.Extensions;
using StoryBlog.Web.Microservices.Comments.Infrastructure.Persistence;
using StoryBlog.Web.Microservices.Comments.WebApi.Configuration;
using StoryBlog.Web.Microservices.Comments.WebApi.Controllers;
using StoryBlog.Web.Microservices.Comments.WebApi.Core;
using StoryBlog.Web.Microservices.Comments.WebApi.Extensions;
using TestProject1.Controllers.Stub;

namespace TestProject1.Controllers.Comments;

public abstract class TestComments : TestControllers
{
    protected IServiceProvider ServiceProvider
    {
        get;
        set;
    }

    protected CommentsController Controller
    {
        get;
        set;
    }

    public override async Task ArrangeAsync()
    {
        await base.ArrangeAsync();

        Services.AddInfrastructureServices();
        //Services.ReplaceUnitOfWork<AsyncUnitOfWorkStub>();
        Services.AddDbContext<CommentsDbContext>(
            options =>
            {
                options
                    .UseInMemoryDatabase("Comments", context =>
                    {
                        context.EnableNullChecks();
                    })
                    .EnableDetailedErrors(detailedErrorsEnabled: true);
            });
        Services.AddApplicationServices();
        Services.AddAutoMapper(configuration =>
        {
            configuration.AddApplicationMappingProfiles();
            configuration.AddWebApiMappingProfiles();
        });
        Services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(ICommentsDbContext).Assembly);
        });
        Services.AddScoped<ILocationProvider, LocationProviderStub>();
        Services
            .AddOptions<CommentLocationProviderOptions>()
            .Configure(options =>
            {
                options.UseUrlHelper = false;
            });

        var serviceProviderFactory = new DefaultServiceProviderFactory();
        ServiceProvider = serviceProviderFactory.CreateServiceProvider(Services);

        Controller = ActivatorUtilities.CreateInstance<CommentsController>(ServiceProvider);
    }
}
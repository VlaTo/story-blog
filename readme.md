[![Build Status](https://dev.azure.com/tolmachewladimir/tolmachewladimir/_apis/build/status/VlaTo.story-blog?branchName=master)](https://dev.azure.com/tolmachewladimir/tolmachewladimir/_build/latest?definitionId=5&branchName=master)

dotnet ef migrations add InitDatabase --context PostsDbContext --output-dir "Persistence\Migrations" --project "..\StoryBlog.Web.Microservices.Posts.Infrastructure" --no-build --verbose
dotnet ef database update --context PostsDbContext --project "..\StoryBlog.Web.Microservices.Posts.Infrastructure" --no-build --verbose

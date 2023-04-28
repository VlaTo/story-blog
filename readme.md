# Story Blog sample web application

[![Build Status](https://dev.azure.com/tolmachewladimir/tolmachewladimir/_apis/build/status/VlaTo.story-blog?branchName=master)](https://dev.azure.com/tolmachewladimir/tolmachewladimir/_build/latest?definitionId=5&branchName=master)
[![.NET](https://github.com/VlaTo/story-blog/actions/workflows/dotnet.yml/badge.svg?branch=master)](https://github.com/VlaTo/story-blog/actions/workflows/dotnet.yml)

Simple Blazor Wasm application with two separate web-services (microservices) with very basic inter-process Message Broker
to communicate each other.

## Working with Database

### Comments Database
```
dotnet ef migrations add InitDatabase --context CommentsDbContext --output-dir "Persistence\Migrations" --project "..\StoryBlog.Web.Microservices.Comments.Infrastructure" --no-build --verbose
dotnet ef database update --context CommentsDbContext --project "..\StoryBlog.Web.Microservices.Comments.Infrastructure" --no-build --verbose
```

### Posts Database
```
dotnet ef migrations add InitDatabase --context PostsDbContext --output-dir "Persistence\Migrations" --project "..\StoryBlog.Web.Microservices.Posts.Infrastructure" --no-build --verbose
dotnet ef database update --context PostsDbContext --project "..\StoryBlog.Web.Microservices.Posts.Infrastructure" --no-build --verbose
```

### Libraries
1. Mediatr
2. SlimMessageBus
3. EntityFrameworkCore
4. Swagger

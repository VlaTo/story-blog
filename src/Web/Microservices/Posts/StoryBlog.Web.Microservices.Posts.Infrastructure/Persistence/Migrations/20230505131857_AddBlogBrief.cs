

#nullable disable

using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore.Migrations;
namespace StoryBlog.Web.Microservices.Posts.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBlogBrief : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brief",
                table: "Contents",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                "Contents",
                "PostId",
                1,
                new[] { "Brief" },
                new object[]
                {
                    "Hudson is the template number 4660 in BTemplates, where a total of 0 templates has been downloaded so far since 2008. This template was created by NBThemes and is the 111th template published in BTemplates.com by this author.\r\nDownload Hudson and more Blogger Templatesom\r\nA normal paragraph\r\nEa eam labores imperdiet, apeirian democritum ei nam, doming neglegentur ad vis."
                }
            );

            migrationBuilder.UpdateData(
                "Contents",
                "PostId",
                2,
                new[] { "Brief" },
                new object[]
                {
                    "Hudson is the template number 4660 in BTemplates, where a total of 0 templates has been downloaded so far since 2008. This template was created by NBThemes and is the 111th template published in BTemplates.com by this author.\r\nDownload Hudson and more Blogger Templatesom\r\nA normal paragraph\r\nEa eam labores imperdiet, apeirian democritum ei nam, doming neglegentur ad vis."
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brief",
                table: "Contents");
        }
    }
}

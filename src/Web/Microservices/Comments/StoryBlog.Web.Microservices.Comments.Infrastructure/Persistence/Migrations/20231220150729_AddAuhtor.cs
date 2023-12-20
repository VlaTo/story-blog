using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryBlog.Web.Microservices.Comments.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuhtor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                schema: "Comment",
                table: "Comments",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorId",
                schema: "Comment",
                table: "Comments");
        }
    }
}

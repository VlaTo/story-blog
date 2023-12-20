using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryBlog.Web.Microservices.Posts.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddVisibilityStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "Blog",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                schema: "Blog",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "Blog",
                table: "Posts",
                newName: "VisibilityStatus");

            migrationBuilder.AddColumn<int>(
                name: "PublicationStatus",
                schema: "Blog",
                table: "Posts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"CREATE FUNCTION ""Posts_Update_ModifiedAt_Function""() RETURNS TRIGGER LANGUAGE PLPGSQL AS $$ BEGIN NEW.""ModifiedAt"" := now(); RETURN NEW; END; $$;");
            migrationBuilder.Sql(@"CREATE TRIGGER ""Update_ModifiedAt"" BEFORE UPDATE ON ""Blog"".""Posts"" FOR EACH ROW EXECUTE FUNCTION ""Posts_Update_ModifiedAt_Function""();");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicationStatus",
                schema: "Blog",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "VisibilityStatus",
                schema: "Blog",
                table: "Posts",
                newName: "Status");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                schema: "Blog",
                table: "Posts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                schema: "Blog",
                table: "Posts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryBlog.Web.Microservices.Comments.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddVisibilityStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "Comment",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                schema: "Comment",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "Comment",
                table: "Comments",
                newName: "VisibilityStatus");

            migrationBuilder.AddColumn<int>(
                name: "PublicationStatus",
                schema: "Comment",
                table: "Comments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"CREATE FUNCTION ""Comments_Update_ModifiedAt_Function""() RETURNS TRIGGER LANGUAGE PLPGSQL AS $$ BEGIN NEW.""ModifiedAt"" := now(); RETURN NEW; END; $$;");
            migrationBuilder.Sql(@"CREATE TRIGGER ""Update_ModifiedAt"" BEFORE UPDATE ON ""Comment"".""Comments"" FOR EACH ROW EXECUTE FUNCTION ""Comments_Update_ModifiedAt_Function""();");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicationStatus",
                schema: "Comment",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "VisibilityStatus",
                schema: "Comment",
                table: "Comments",
                newName: "Status");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                schema: "Comment",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                schema: "Comment",
                table: "Comments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}

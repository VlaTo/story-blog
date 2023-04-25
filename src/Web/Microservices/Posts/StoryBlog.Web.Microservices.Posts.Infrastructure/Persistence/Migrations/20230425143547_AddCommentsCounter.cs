using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryBlog.Web.Microservices.Posts.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentsCounter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommentsCounters",
                columns: table => new
                {
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    Counter = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentsCounters", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_CommentsCounters_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentsCounters_PostId",
                table: "CommentsCounters",
                column: "PostId",
                unique: true);

            migrationBuilder.InsertData(
                "CommentsCounters",
                new[] { "PostId", "Counter" },
                new object[]
                {
                    1, // Id
                    2
                }
            );

            migrationBuilder.InsertData(
                "CommentsCounters",
                new[] { "PostId", "Counter" },
                new object[]
                {
                    2, // Id
                    1
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentsCounters");
        }
    }
}

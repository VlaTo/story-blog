using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryBlog.Web.Microservices.Posts.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Slugs",
                columns: table => new
                {
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slugs", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Slugs_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Key",
                table: "Posts",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Title",
                table: "Posts",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Slugs_Text",
                table: "Slugs",
                column: "Text",
                unique: true);

            migrationBuilder.InsertData(
                "Posts",
                new[] { "Id", "Key", "Title", "IsPublic", "Status", "CreateAt" },
                new object[]
                {
                    1, // Id
                    Guid.Parse("8eb975fc-74d2-4be1-bd95-f25ea308bd3b"), // Key
                    "Lorem Ipsum dolor sit amet 8eb975fc-74d2-4be1-bd95-f25ea308bd3b",
                    true,
                    1,
                    new DateTime(2023, 4, 21, 10, 17, 0)
                }
            );

            migrationBuilder.InsertData(
                "Slugs",
                new[] { "PostId", "Text" },
                new object[]
                {
                    1, // Id
                    "lorem-ipsum-dolor-sit-amet-8eb975fc-74d2-4be1-bd95-f25ea308bd3b"
                }
            );

            migrationBuilder.InsertData(
                "Posts",
                new[] { "Id", "Key", "Title", "IsPublic", "Status", "CreateAt" },
                new object[]
                {
                    2, // Id
                    Guid.Parse("3e406eae-11f4-4dee-98f4-921f9be80bb3"), // Key
                    "Lorem Ipsum dolor sit amet 3e406eae-11f4-4dee-98f4-921f9be80bb3",
                    true,
                    1,
                    new DateTime(2023, 4, 21, 10, 19, 0)
                }
            );

            migrationBuilder.InsertData(
                "Slugs",
                new[] { "PostId", "Text" },
                new object[]
                {
                    2, // Id
                    "lorem-ipsum-dolor-sit-amet-3e406eae-11f4-4dee-98f4-921f9be80bb3"
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Slugs");

            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryBlog.Web.Microservices.Comments.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    PostKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Comments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentId",
                table: "Comments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostKey",
                table: "Comments",
                column: "PostKey");

            migrationBuilder.InsertData(
                "Comments",
                new[] { "Id", "Key", "ParentId", "PostKey", "Text", "Status", "IsPublic", "CreateAt" },
                new object[]
                {
                    1,  // Id
                    Guid.Parse("a2d32742-99ac-405c-bce8-b3054a8ff3d6"), // Key
                    null,   // ParentId
                    Guid.Parse("8eb975fc-74d2-4be1-bd95-f25ea308bd3b"), // PostKey
                    "Lorem Ipsum dolor sit amet 8eb975fc-74d2-4be1-bd95-f25ea308bd3b",
                    1,
                    true,
                    new DateTime(2023, 4, 21, 10, 17, 0)
                }
            );

            migrationBuilder.InsertData(
                "Comments",
                new[] { "Id", "Key", "ParentId", "PostKey", "Text", "Status", "IsPublic", "CreateAt" },
                new object[]
                {
                        2,  // Id
                        Guid.Parse("7daa7f64-de69-4aed-b67a-f49bca922d37"), // Key
                        1, // ParentId
                        Guid.Parse("8eb975fc-74d2-4be1-bd95-f25ea308bd3b"), // PostKey
                        "Lorem Ipsum dolor sit amet 8eb975fc-74d2-4be1-bd95-f25ea308bd3b",
                        1,
                        true,
                        new DateTime(2023, 4, 21, 10, 20, 0)
                }
            );

            migrationBuilder.InsertData(
                "Comments",
                new[] { "Id", "Key", "ParentId", "PostKey", "Text", "Status", "IsPublic", "CreateAt" },
                new object[]
                {
                    3,  // Id
                    Guid.Parse("3ebb5ce8-d664-4927-8df5-e26a04c85c8c"), // Key
                    null,   // ParentId
                    Guid.Parse("8eb975fc-74d2-4be1-bd95-f25ea308bd3b"), // PostKey
                    "Lorem Ipsum dolor sit amet 8eb975fc-74d2-4be1-bd95-f25ea308bd3b",
                    1,
                    true,
                    new DateTime(2023, 4, 21, 10, 18, 0)
                }
            );

            migrationBuilder.InsertData(
                "Comments",
                new[] { "Id", "Key", "ParentId", "PostKey", "Text", "Status", "IsPublic", "CreateAt" },
                new object[]
                {
                    4,  // Id
                    Guid.Parse("01605f80-38b7-4b82-bf03-101de7b89114"), // Key
                    null,   // ParentId
                    Guid.Parse("3e406eae-11f4-4dee-98f4-921f9be80bb3"), // PostKey
                    "Lorem Ipsum dolor sit amet 3e406eae-11f4-4dee-98f4-921f9be80bb3",
                    1,
                    true,
                    new DateTime(2023, 4, 21, 10, 19, 0)
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");
        }
    }
}

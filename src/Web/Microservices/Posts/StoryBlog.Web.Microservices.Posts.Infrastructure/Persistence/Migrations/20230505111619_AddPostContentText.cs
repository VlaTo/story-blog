using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryBlog.Web.Microservices.Posts.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPostContentText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contents",
                columns: table => new
                {
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    Text = table.Column<string>(type: "ntext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Contents_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                "Contents",
                new[] { "PostId", "Text" },
                new object[]
                {
                    1, // Id
                    "[Hudson](https://btemplates.com/2016/blogger-template-hudson/) is the template number 4660 in BTemplates, where a total of 0 templates has been downloaded so far since 2008. This template was created by [NBThemes](https://btemplates.com/author/new-blogger-themes/) and is the 111th template published in BTemplates.com by this author.\r\n\r\nDownload [Hudson](https://btemplates.com/2016/blogger-template-hudson/) and more [Blogger Templates](http://btemplates.com/) at BTemplates ![BTemplates](http://1.bp.blogspot.com/_zNpQsOlwuPA/S2lKsFgNlMI/AAAAAAAAAKw/0Qay8rrjLKQ/s000/btemplates-small.png)\r\n\r\n### A normal paragraph\r\nEa eam labores imperdiet, apeirian democritum ei nam, doming neglegentur ad vis. Ne malorum ceteros feugait quo, ius ea liber offendit placerat, est habemus aliquyam legendos id. Eam no corpora maluisset definitiones, eam mucius malorum id. Quo ea idque commodo utroque, per ex eros etiam accumsan.\r\n\r\n### A paragraph with format\r\nEt posse meliore **definitiones (strong)** his, vim _tritani vulputate (italic)_ pertinacia at. Augue quaerendum (Acronym) te sea, ex sed sint ~~invenire erroribus~~. Cu vel ceteros scripserit, te usu modus fabellas mediocritatem. In legere regione instructior eos. Ea repudiandae suscipiantur vim, vel partem labores ponderum in [blogger templates](http://btemplates.com/).(link)\r\n\r\n### A paragraph as code\r\n```\r\nMel putent quaeque an, ut postea melius denique sit. Officiis sensibus at mea,\r\nsea at labitur deserunt. Eam dicam congue soluta ut.\r\n```\r\n\r\n### A paragraph as blockquote.\r\n>Eu mei solum oporteat eleifend, libris nominavi maiestatis duo at,\r\n>quod dissentiet vel te. Legere prompta impedit id eum. Te soleat\r\n>vocibus luptatum sed, augue dicta populo est ad, et consul diceret officiis duo. Et duo primis nostrum.\r\n\r\n### Unordered list\r\n* Blogger templates\r\n* Templates\r\n* Blogs\r\n* Layouts\r\n* Skins\r\n* BTemplates\r\n\r\n### Ordered list\r\n1. Login in Blogger.com\r\n2. Go to BTemplates and find your perfect Blogger template\r\n3. Download the template and upload it to your blog.\r\n4. Check out the template settings and your own gadgets.\r\n5. That's it! Your blog just got a new design.\r\n\r\n### Heading\r\n# Heading 1\r\n## Heading 2\r\n### Heading 3\r\n#### Heading 4\r\n##### Heading 5\r\n###### Heading 6\r\n\r\n### A table\r\n|Table Header 1|Table Header 2|Table Header 3|\r\n|--------------|--------------|--------------|\r\nDivision 1|Division 2|Division 3|\r\nDivision 1|Division 2|Division 3|\r\nDivision 1|Division 2|Division 3|"
                }
            );

            migrationBuilder.InsertData(
                "Contents",
                new[] { "PostId", "Text" },
                new object[]
                {
                    2, // Id
                    "[Hudson](https://btemplates.com/2016/blogger-template-hudson/) is the template number 4660 in BTemplates, where a total of 0 templates has been downloaded so far since 2008. This template was created by [NBThemes](https://btemplates.com/author/new-blogger-themes/) and is the 111th template published in BTemplates.com by this author.\r\n\r\nDownload [Hudson](https://btemplates.com/2016/blogger-template-hudson/) and more [Blogger Templates](http://btemplates.com/) at BTemplates ![BTemplates](http://1.bp.blogspot.com/_zNpQsOlwuPA/S2lKsFgNlMI/AAAAAAAAAKw/0Qay8rrjLKQ/s000/btemplates-small.png)\r\n\r\n### A normal paragraph\r\nEa eam labores imperdiet, apeirian democritum ei nam, doming neglegentur ad vis. Ne malorum ceteros feugait quo, ius ea liber offendit placerat, est habemus aliquyam legendos id. Eam no corpora maluisset definitiones, eam mucius malorum id. Quo ea idque commodo utroque, per ex eros etiam accumsan.\r\n\r\n### A paragraph with format\r\nEt posse meliore **definitiones (strong)** his, vim _tritani vulputate (italic)_ pertinacia at. Augue quaerendum (Acronym) te sea, ex sed sint ~~invenire erroribus~~. Cu vel ceteros scripserit, te usu modus fabellas mediocritatem. In legere regione instructior eos. Ea repudiandae suscipiantur vim, vel partem labores ponderum in [blogger templates](http://btemplates.com/).(link)\r\n\r\n### A paragraph as code\r\n```\r\nMel putent quaeque an, ut postea melius denique sit. Officiis sensibus at mea,\r\nsea at labitur deserunt. Eam dicam congue soluta ut.\r\n```\r\n\r\n### A paragraph as blockquote.\r\n>Eu mei solum oporteat eleifend, libris nominavi maiestatis duo at,\r\n>quod dissentiet vel te. Legere prompta impedit id eum. Te soleat\r\n>vocibus luptatum sed, augue dicta populo est ad, et consul diceret officiis duo. Et duo primis nostrum.\r\n\r\n### Unordered list\r\n* Blogger templates\r\n* Templates\r\n* Blogs\r\n* Layouts\r\n* Skins\r\n* BTemplates\r\n\r\n### Ordered list\r\n1. Login in Blogger.com\r\n2. Go to BTemplates and find your perfect Blogger template\r\n3. Download the template and upload it to your blog.\r\n4. Check out the template settings and your own gadgets.\r\n5. That's it! Your blog just got a new design.\r\n\r\n### Heading\r\n# Heading 1\r\n## Heading 2\r\n### Heading 3\r\n#### Heading 4\r\n##### Heading 5\r\n###### Heading 6\r\n\r\n### A table\r\n|Table Header 1|Table Header 2|Table Header 3|\r\n|--------------|--------------|--------------|\r\nDivision 1|Division 2|Division 3|\r\nDivision 1|Division 2|Division 3|\r\nDivision 1|Division 2|Division 3|"
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contents");
        }
    }
}

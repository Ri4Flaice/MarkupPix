using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace MarkupPix.Data.Migrations
{
    /// <inheritdoc />
    public partial class PagesTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false, comment: "Document id"),
                    IsRecognize = table.Column<bool>(type: "tinyint(1)", nullable: true, comment: "Indicates that the page is recognized"),
                    RecognizeUser = table.Column<long>(type: "bigint", nullable: true, comment: "The user who recognized the page."),
                    DateRecognize = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "Date of recognition"),
                    Page = table.Column<byte[]>(type: "longblob", nullable: true, comment: "Page")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pages_AspNetUsers_RecognizeUser",
                        column: x => x.RecognizeUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_pages_documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Pages data")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_pages_DocumentId",
                table: "pages",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_pages_RecognizeUser",
                table: "pages",
                column: "RecognizeUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pages");
        }
    }
}

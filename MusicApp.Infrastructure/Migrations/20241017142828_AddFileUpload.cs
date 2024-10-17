using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFileUpload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AudioFileId",
                table: "Tracks",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "CoverImageId",
                table: "Tracks",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "FileUploads",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FileName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUploads", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_AudioFileId",
                table: "Tracks",
                column: "AudioFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_CoverImageId",
                table: "Tracks",
                column: "CoverImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_FileUploads_AudioFileId",
                table: "Tracks",
                column: "AudioFileId",
                principalTable: "FileUploads",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_FileUploads_CoverImageId",
                table: "Tracks",
                column: "CoverImageId",
                principalTable: "FileUploads",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_FileUploads_AudioFileId",
                table: "Tracks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_FileUploads_CoverImageId",
                table: "Tracks");

            migrationBuilder.DropTable(
                name: "FileUploads");

            migrationBuilder.DropIndex(
                name: "IX_Tracks_AudioFileId",
                table: "Tracks");

            migrationBuilder.DropIndex(
                name: "IX_Tracks_CoverImageId",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "AudioFileId",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "CoverImageId",
                table: "Tracks");
        }
    }
}

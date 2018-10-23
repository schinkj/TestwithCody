using Microsoft.EntityFrameworkCore.Migrations;

namespace solution_MVC_Music.Data.MusicMigrations
{
    public partial class fixGenres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Genre_GenreID",
                schema: "MUSIC",
                table: "Albums");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Genre_GenreID",
                schema: "MUSIC",
                table: "Songs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genre",
                schema: "MUSIC",
                table: "Genre");

            migrationBuilder.RenameTable(
                name: "Genre",
                schema: "MUSIC",
                newName: "Genres",
                newSchema: "MUSIC");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genres",
                schema: "MUSIC",
                table: "Genres",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Genres_GenreID",
                schema: "MUSIC",
                table: "Albums",
                column: "GenreID",
                principalSchema: "MUSIC",
                principalTable: "Genres",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Genres_GenreID",
                schema: "MUSIC",
                table: "Songs",
                column: "GenreID",
                principalSchema: "MUSIC",
                principalTable: "Genres",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Genres_GenreID",
                schema: "MUSIC",
                table: "Albums");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Genres_GenreID",
                schema: "MUSIC",
                table: "Songs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genres",
                schema: "MUSIC",
                table: "Genres");

            migrationBuilder.RenameTable(
                name: "Genres",
                schema: "MUSIC",
                newName: "Genre",
                newSchema: "MUSIC");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genre",
                schema: "MUSIC",
                table: "Genre",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Genre_GenreID",
                schema: "MUSIC",
                table: "Albums",
                column: "GenreID",
                principalSchema: "MUSIC",
                principalTable: "Genre",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Genre_GenreID",
                schema: "MUSIC",
                table: "Songs",
                column: "GenreID",
                principalSchema: "MUSIC",
                principalTable: "Genre",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace solution_MVC_Music.Data.MusicMigrations
{
    public partial class uniqueAlbum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Albums_Name_YearProduced",
                schema: "MUSIC",
                table: "Albums",
                columns: new[] { "Name", "YearProduced" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Albums_Name_YearProduced",
                schema: "MUSIC",
                table: "Albums");
        }
    }
}

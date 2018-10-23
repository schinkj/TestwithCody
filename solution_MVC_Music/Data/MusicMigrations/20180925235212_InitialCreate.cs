using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace solution_MVC_Music.Data.MusicMigrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "MUSIC");

            migrationBuilder.CreateTable(
                name: "Genre",
                schema: "MUSIC",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Instruments",
                schema: "MUSIC",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instruments", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                schema: "MUSIC",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GenreID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Albums_Genre_GenreID",
                        column: x => x.GenreID,
                        principalSchema: "MUSIC",
                        principalTable: "Genre",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Musicians",
                schema: "MUSIC",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SIN = table.Column<string>(maxLength: 9, nullable: true),
                    InstrumentID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musicians", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Musicians_Instruments_InstrumentID",
                        column: x => x.InstrumentID,
                        principalSchema: "MUSIC",
                        principalTable: "Instruments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                schema: "MUSIC",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AlbumID = table.Column<int>(nullable: false),
                    GenreID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Songs_Albums_AlbumID",
                        column: x => x.AlbumID,
                        principalSchema: "MUSIC",
                        principalTable: "Albums",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Songs_Genre_GenreID",
                        column: x => x.GenreID,
                        principalSchema: "MUSIC",
                        principalTable: "Genre",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Plays",
                schema: "MUSIC",
                columns: table => new
                {
                    InstrumentID = table.Column<int>(nullable: false),
                    MusicianID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plays", x => new { x.MusicianID, x.InstrumentID });
                    table.ForeignKey(
                        name: "FK_Plays_Instruments_InstrumentID",
                        column: x => x.InstrumentID,
                        principalSchema: "MUSIC",
                        principalTable: "Instruments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plays_Musicians_MusicianID",
                        column: x => x.MusicianID,
                        principalSchema: "MUSIC",
                        principalTable: "Musicians",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Performances",
                schema: "MUSIC",
                columns: table => new
                {
                    SongID = table.Column<int>(nullable: false),
                    MusicianID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Performances", x => new { x.MusicianID, x.SongID });
                    table.ForeignKey(
                        name: "FK_Performances_Musicians_MusicianID",
                        column: x => x.MusicianID,
                        principalSchema: "MUSIC",
                        principalTable: "Musicians",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Performances_Songs_SongID",
                        column: x => x.SongID,
                        principalSchema: "MUSIC",
                        principalTable: "Songs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Albums_GenreID",
                schema: "MUSIC",
                table: "Albums",
                column: "GenreID");

            migrationBuilder.CreateIndex(
                name: "IX_Musicians_InstrumentID",
                schema: "MUSIC",
                table: "Musicians",
                column: "InstrumentID");

            migrationBuilder.CreateIndex(
                name: "IX_Musicians_SIN",
                schema: "MUSIC",
                table: "Musicians",
                column: "SIN",
                unique: true,
                filter: "[SIN] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Performances_SongID",
                schema: "MUSIC",
                table: "Performances",
                column: "SongID");

            migrationBuilder.CreateIndex(
                name: "IX_Plays_InstrumentID",
                schema: "MUSIC",
                table: "Plays",
                column: "InstrumentID");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_AlbumID",
                schema: "MUSIC",
                table: "Songs",
                column: "AlbumID");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_GenreID",
                schema: "MUSIC",
                table: "Songs",
                column: "GenreID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Performances",
                schema: "MUSIC");

            migrationBuilder.DropTable(
                name: "Plays",
                schema: "MUSIC");

            migrationBuilder.DropTable(
                name: "Songs",
                schema: "MUSIC");

            migrationBuilder.DropTable(
                name: "Musicians",
                schema: "MUSIC");

            migrationBuilder.DropTable(
                name: "Albums",
                schema: "MUSIC");

            migrationBuilder.DropTable(
                name: "Instruments",
                schema: "MUSIC");

            migrationBuilder.DropTable(
                name: "Genre",
                schema: "MUSIC");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace solution_MVC_Music.Data.MusicMigrations
{
    public partial class DataProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Musicians_SIN",
                schema: "MUSIC",
                table: "Musicians");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "MUSIC",
                table: "Songs",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "SIN",
                schema: "MUSIC",
                table: "Musicians",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 9,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DOB",
                schema: "MUSIC",
                table: "Musicians",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "MUSIC",
                table: "Musicians",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "MUSIC",
                table: "Musicians",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                schema: "MUSIC",
                table: "Musicians",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Phone",
                schema: "MUSIC",
                table: "Musicians",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "MUSIC",
                table: "Instruments",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "MUSIC",
                table: "Genre",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "MUSIC",
                table: "Albums",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                schema: "MUSIC",
                table: "Albums",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "YearProduced",
                schema: "MUSIC",
                table: "Albums",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Musicians_SIN",
                schema: "MUSIC",
                table: "Musicians",
                column: "SIN",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Musicians_SIN",
                schema: "MUSIC",
                table: "Musicians");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "MUSIC",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "DOB",
                schema: "MUSIC",
                table: "Musicians");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "MUSIC",
                table: "Musicians");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "MUSIC",
                table: "Musicians");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                schema: "MUSIC",
                table: "Musicians");

            migrationBuilder.DropColumn(
                name: "Phone",
                schema: "MUSIC",
                table: "Musicians");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "MUSIC",
                table: "Instruments");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "MUSIC",
                table: "Genre");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "MUSIC",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "Price",
                schema: "MUSIC",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "YearProduced",
                schema: "MUSIC",
                table: "Albums");

            migrationBuilder.AlterColumn<string>(
                name: "SIN",
                schema: "MUSIC",
                table: "Musicians",
                maxLength: 9,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 9);

            migrationBuilder.CreateIndex(
                name: "IX_Musicians_SIN",
                schema: "MUSIC",
                table: "Musicians",
                column: "SIN",
                unique: true,
                filter: "[SIN] IS NOT NULL");
        }
    }
}

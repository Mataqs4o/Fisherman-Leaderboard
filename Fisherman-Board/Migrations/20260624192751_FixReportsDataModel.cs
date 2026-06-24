using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fisherman_Board.Migrations
{
    /// <inheritdoc />
    public partial class FixReportsDataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPermitRevoked",
                table: "FishingVessels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PermitValidTo",
                table: "FishingVessels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
    name: "PersonId",
    table: "CatchRecords",
    type: "int",
    nullable: false,
    defaultValue: 0);

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM People)
BEGIN
    INSERT INTO People (FullName, BirthDate, IsPensioner, IsDisabled, TelkNumber)
    VALUES (N'Неизвестен рибар', '2000-01-01', 0, 0, NULL)
END

UPDATE CatchRecords
SET PersonId = (SELECT TOP 1 Id FROM People ORDER BY Id)
WHERE PersonId = 0
   OR PersonId NOT IN (SELECT Id FROM People)
");

            migrationBuilder.CreateIndex(
                name: "IX_CatchRecords_PersonId",
                table: "CatchRecords",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_CatchRecords_People_PersonId",
                table: "CatchRecords",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatchRecords_People_PersonId",
                table: "CatchRecords");

            migrationBuilder.DropIndex(
                name: "IX_CatchRecords_PersonId",
                table: "CatchRecords");

            migrationBuilder.DropColumn(
                name: "IsPermitRevoked",
                table: "FishingVessels");

            migrationBuilder.DropColumn(
                name: "PermitValidTo",
                table: "FishingVessels");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "CatchRecords");
        }
    }
}

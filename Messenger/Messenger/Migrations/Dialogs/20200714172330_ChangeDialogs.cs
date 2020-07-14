using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Messenger.Migrations.Dialogs
{
    public partial class ChangeDialogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Creator",
                schema: "Dialog",
                table: "Dialogs",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Dialog",
                table: "Dialogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                schema: "Dialog",
                table: "Dialogs");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Dialog",
                table: "Dialogs");
        }
    }
}

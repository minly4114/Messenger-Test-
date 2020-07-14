using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Messenger.Migrations.Dialogs
{
    public partial class ChangeTypeParticipants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<string>>(
                name: "Participants",
                schema: "Dialog",
                table: "Dialogs",
                nullable: true,
                oldClrType: typeof(Guid[]),
                oldType: "uuid[]",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid[]>(
                name: "Participants",
                schema: "Dialog",
                table: "Dialogs",
                type: "uuid[]",
                nullable: true,
                oldClrType: typeof(List<string>),
                oldNullable: true);
        }
    }
}

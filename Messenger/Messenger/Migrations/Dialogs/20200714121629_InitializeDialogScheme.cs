using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Messenger.Migrations.Dialogs
{
    public partial class InitializeDialogScheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Dialog");

            migrationBuilder.CreateTable(
                name: "Dialogs",
                schema: "Dialog",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(nullable: false),
                    Participants = table.Column<List<Guid>>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dialogs", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                schema: "Dialog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DialogUuid = table.Column<Guid>(nullable: true),
                    Sender = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Dialogs_DialogUuid",
                        column: x => x.DialogUuid,
                        principalSchema: "Dialog",
                        principalTable: "Dialogs",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Read",
                schema: "Dialog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UuidParticipant = table.Column<Guid>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false),
                    MessageId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Read", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Read_Messages_MessageId",
                        column: x => x.MessageId,
                        principalSchema: "Dialog",
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DialogUuid",
                schema: "Dialog",
                table: "Messages",
                column: "DialogUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Read_MessageId",
                schema: "Dialog",
                table: "Read",
                column: "MessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Read",
                schema: "Dialog");

            migrationBuilder.DropTable(
                name: "Messages",
                schema: "Dialog");

            migrationBuilder.DropTable(
                name: "Dialogs",
                schema: "Dialog");
        }
    }
}

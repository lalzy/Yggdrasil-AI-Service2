using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yggdrasil_ai_service.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemPrompt",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPrompt", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "World",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Lorebook_ID = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    IntroMessage = table.Column<string>(type: "TEXT", nullable: true),
                    NarratorInstruction = table.Column<string>(type: "TEXT", nullable: true),
                    Scenario = table.Column<string>(type: "TEXT", nullable: true),
                    NarratorExampleDialogue = table.Column<string>(type: "TEXT", nullable: false),
                    LastUsed = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_World", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Prompt",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    Source = table.Column<int>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    SystemPromptID = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prompt", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Prompt_SystemPrompt_SystemPromptID",
                        column: x => x.SystemPromptID,
                        principalTable: "SystemPrompt",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Theme = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultPromptID = table.Column<Guid>(type: "TEXT", nullable: false),
                    ActivePromptID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Settings_SystemPrompt_ActivePromptID",
                        column: x => x.ActivePromptID,
                        principalTable: "SystemPrompt",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Settings_SystemPrompt_DefaultPromptID",
                        column: x => x.DefaultPromptID,
                        principalTable: "SystemPrompt",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Character",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    NarrativeRole = table.Column<string>(type: "TEXT", nullable: true),
                    Personality = table.Column<string>(type: "TEXT", nullable: false),
                    ExampleDialogue = table.Column<string>(type: "TEXT", nullable: true),
                    Conversation_ID = table.Column<Guid>(type: "TEXT", nullable: true),
                    WorldID = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Gender = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Pronouns = table.Column<string>(type: "TEXT", nullable: true),
                    FullName = table.Column<string>(type: "TEXT", nullable: true),
                    Race = table.Column<string>(type: "TEXT", nullable: true),
                    Occupation = table.Column<string>(type: "TEXT", nullable: true),
                    Appearance = table.Column<string>(type: "TEXT", nullable: true),
                    Equipment = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Character", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Character_World_WorldID",
                        column: x => x.WorldID,
                        principalTable: "World",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Character_WorldID",
                table: "Character",
                column: "WorldID");

            migrationBuilder.CreateIndex(
                name: "IX_Prompt_SystemPromptID",
                table: "Prompt",
                column: "SystemPromptID");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_ActivePromptID",
                table: "Settings",
                column: "ActivePromptID");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_DefaultPromptID",
                table: "Settings",
                column: "DefaultPromptID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Character");

            migrationBuilder.DropTable(
                name: "Prompt");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "World");

            migrationBuilder.DropTable(
                name: "SystemPrompt");
        }
    }
}

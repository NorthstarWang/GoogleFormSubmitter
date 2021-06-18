using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace M8OU.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OptionColumn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormQuestionOptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionColumn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionColumn_FormQuestionOption_FormQuestionOptionId",
                        column: x => x.FormQuestionOptionId,
                        principalTable: "FormQuestionOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OptionColumn_FormQuestionOptionId",
                table: "OptionColumn",
                column: "FormQuestionOptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OptionColumn");
        }
    }
}

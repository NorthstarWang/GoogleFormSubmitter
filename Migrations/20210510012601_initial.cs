using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace M8OU.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_AspNetUsers_IdentityUserId",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_FormHistories_FormHistoryId",
                table: "Report");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Report",
                table: "Report");

            migrationBuilder.RenameTable(
                name: "Report",
                newName: "Reports");

            migrationBuilder.RenameIndex(
                name: "IX_Report_IdentityUserId",
                table: "Reports",
                newName: "IX_Reports_IdentityUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Report_FormHistoryId",
                table: "Reports",
                newName: "IX_Reports_FormHistoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reports",
                table: "Reports",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FormQuestion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionNumber = table.Column<int>(type: "int", nullable: false),
                    QuestionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalSubmissionNumber = table.Column<int>(type: "int", nullable: false),
                    QuestionContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormQuestion_FormHistories_FormHistoryId",
                        column: x => x.FormHistoryId,
                        principalTable: "FormHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormQuestionOption",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmissionNumber = table.Column<int>(type: "int", nullable: false),
                    FormQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormQuestionOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormQuestionOption_FormQuestion_FormQuestionId",
                        column: x => x.FormQuestionId,
                        principalTable: "FormQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormQuestion_FormHistoryId",
                table: "FormQuestion",
                column: "FormHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FormQuestionOption_FormQuestionId",
                table: "FormQuestionOption",
                column: "FormQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_AspNetUsers_IdentityUserId",
                table: "Reports",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_FormHistories_FormHistoryId",
                table: "Reports",
                column: "FormHistoryId",
                principalTable: "FormHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_AspNetUsers_IdentityUserId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_FormHistories_FormHistoryId",
                table: "Reports");

            migrationBuilder.DropTable(
                name: "FormQuestionOption");

            migrationBuilder.DropTable(
                name: "FormQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reports",
                table: "Reports");

            migrationBuilder.RenameTable(
                name: "Reports",
                newName: "Report");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_IdentityUserId",
                table: "Report",
                newName: "IX_Report_IdentityUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_FormHistoryId",
                table: "Report",
                newName: "IX_Report_FormHistoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Report",
                table: "Report",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_AspNetUsers_IdentityUserId",
                table: "Report",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_FormHistories_FormHistoryId",
                table: "Report",
                column: "FormHistoryId",
                principalTable: "FormHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

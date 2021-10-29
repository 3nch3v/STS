using Microsoft.EntityFrameworkCore.Migrations;

namespace STS.Data.Migrations
{
    public partial class AddNewPropertiesAssignedToForTicketsAndManagerForEmplTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedToId",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "EmployeesTasks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AssignedToId",
                table: "Tickets",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeesTasks_ManagerId",
                table: "EmployeesTasks",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeesTasks_AspNetUsers_ManagerId",
                table: "EmployeesTasks",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_AssignedToId",
                table: "Tickets",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeesTasks_AspNetUsers_ManagerId",
                table: "EmployeesTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_AssignedToId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_AssignedToId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_EmployeesTasks_ManagerId",
                table: "EmployeesTasks");

            migrationBuilder.DropColumn(
                name: "AssignedToId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "EmployeesTasks");
        }
    }
}

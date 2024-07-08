using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatetovisitorstable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visitors_Employees_HostId",
                table: "Visitors");

            migrationBuilder.DropIndex(
                name: "IX_Visitors_ContactNumber",
                table: "Visitors");

            migrationBuilder.AlterColumn<int>(
                name: "HostId",
                table: "Visitors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ContactNumber",
                table: "Visitors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Visitors_Employees_HostId",
                table: "Visitors",
                column: "HostId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visitors_Employees_HostId",
                table: "Visitors");

            migrationBuilder.AlterColumn<int>(
                name: "HostId",
                table: "Visitors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactNumber",
                table: "Visitors",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_ContactNumber",
                table: "Visitors",
                column: "ContactNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Visitors_Employees_HostId",
                table: "Visitors",
                column: "HostId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

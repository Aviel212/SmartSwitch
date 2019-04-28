using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEnd.Migrations
{
    public partial class AddPlugsToContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plug_Users_UserName",
                table: "Plug");

            migrationBuilder.DropForeignKey(
                name: "FK_PowerUsageSample_Plug_PlugMac",
                table: "PowerUsageSample");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Plug_DeviceMac",
                table: "Task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Plug",
                table: "Plug");

            migrationBuilder.RenameTable(
                name: "Plug",
                newName: "Plugs");

            migrationBuilder.RenameIndex(
                name: "IX_Plug_UserName",
                table: "Plugs",
                newName: "IX_Plugs_UserName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Plugs",
                table: "Plugs",
                column: "Mac");

            migrationBuilder.AddForeignKey(
                name: "FK_Plugs_Users_UserName",
                table: "Plugs",
                column: "UserName",
                principalTable: "Users",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PowerUsageSample_Plugs_PlugMac",
                table: "PowerUsageSample",
                column: "PlugMac",
                principalTable: "Plugs",
                principalColumn: "Mac",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Plugs_DeviceMac",
                table: "Task",
                column: "DeviceMac",
                principalTable: "Plugs",
                principalColumn: "Mac",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plugs_Users_UserName",
                table: "Plugs");

            migrationBuilder.DropForeignKey(
                name: "FK_PowerUsageSample_Plugs_PlugMac",
                table: "PowerUsageSample");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Plugs_DeviceMac",
                table: "Task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Plugs",
                table: "Plugs");

            migrationBuilder.RenameTable(
                name: "Plugs",
                newName: "Plug");

            migrationBuilder.RenameIndex(
                name: "IX_Plugs_UserName",
                table: "Plug",
                newName: "IX_Plug_UserName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Plug",
                table: "Plug",
                column: "Mac");

            migrationBuilder.AddForeignKey(
                name: "FK_Plug_Users_UserName",
                table: "Plug",
                column: "UserName",
                principalTable: "Users",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PowerUsageSample_Plug_PlugMac",
                table: "PowerUsageSample",
                column: "PlugMac",
                principalTable: "Plug",
                principalColumn: "Mac",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Plug_DeviceMac",
                table: "Task",
                column: "DeviceMac",
                principalTable: "Plug",
                principalColumn: "Mac",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

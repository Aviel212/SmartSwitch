using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEnd.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "Plug",
                columns: table => new
                {
                    Mac = table.Column<string>(nullable: false),
                    Nickname = table.Column<string>(nullable: true),
                    IsOn = table.Column<bool>(nullable: false),
                    Approved = table.Column<bool>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plug", x => x.Mac);
                    table.ForeignKey(
                        name: "FK_Plug_Users_UserName",
                        column: x => x.UserName,
                        principalTable: "Users",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    TaskId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Operation = table.Column<int>(nullable: false),
                    DeviceMac = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.TaskId);
                    table.ForeignKey(
                        name: "FK_Task_Plug_DeviceMac",
                        column: x => x.DeviceMac,
                        principalTable: "Plug",
                        principalColumn: "Mac",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plug_UserName",
                table: "Plug",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_Task_DeviceMac",
                table: "Task",
                column: "DeviceMac");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "Plug");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

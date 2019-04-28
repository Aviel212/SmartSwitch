using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEnd.Migrations
{
    public partial class AddSamples : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PowerUsageSample",
                columns: table => new
                {
                    SampleDate = table.Column<DateTime>(nullable: false),
                    Current = table.Column<double>(nullable: false),
                    Voltage = table.Column<double>(nullable: false),
                    PlugMac = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerUsageSample", x => x.SampleDate);
                    table.ForeignKey(
                        name: "FK_PowerUsageSample_Plug_PlugMac",
                        column: x => x.PlugMac,
                        principalTable: "Plug",
                        principalColumn: "Mac",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PowerUsageSample_PlugMac",
                table: "PowerUsageSample",
                column: "PlugMac");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PowerUsageSample");
        }
    }
}

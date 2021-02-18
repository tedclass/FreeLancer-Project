using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FreeLancers4.Migrations.FreeLancers4New
{
    public partial class ProjectTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Work",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectTitle = table.Column<string>(nullable: true),
                    PostDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Techneeded = table.Column<string>(nullable: true),
                    Skills = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    TimeFrame = table.Column<int>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Work", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Work");
        }
    }
}

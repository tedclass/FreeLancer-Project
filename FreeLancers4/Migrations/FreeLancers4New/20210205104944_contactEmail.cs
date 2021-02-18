using Microsoft.EntityFrameworkCore.Migrations;

namespace FreeLancers4.Migrations.FreeLancers4New
{
    public partial class contactEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "contactEmail",
                table: "Work",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contactEmail",
                table: "Work");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolProject.Migrations
{
    public partial class ModifyClassEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SchoolId",
                table: "ClassEntities",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "ClassEntities");
        }
    }
}

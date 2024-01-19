using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeProject.Migrations
{
    public partial class BeforeUserAllergen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRecipeWriter",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRecipeWriter",
                table: "AspNetUsers");
        }
    }
}

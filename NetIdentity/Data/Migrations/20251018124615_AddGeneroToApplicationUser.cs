using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetIdentity.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGeneroToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Genero",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Genero",
                table: "AspNetUsers");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AplicacionPedidos.Migrations
{
    /// <inheritdoc />
    public partial class AgregarColumnasFaltantesPedidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CustomUsers_ClienteId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ClienteId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "ImagenUrl",
                table: "Products",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Products",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Categoria",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "DireccionEntrega",
                table: "Orders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notas",
                table: "Orders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioUnitario",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CustomUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "CustomUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CustomUsers_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DireccionEntrega",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Notas",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PrecioUnitario",
                table: "OrderItems");

            migrationBuilder.AlterColumn<string>(
                name: "ImagenUrl",
                table: "Products",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Products",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Categoria",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClienteId",
                table: "Orders",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CustomUsers_ClienteId",
                table: "Orders",
                column: "ClienteId",
                principalTable: "CustomUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Class_22_Review.Migrations
{
    public partial class Atualizando : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Products_ProductId1",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_ProductId1",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "Tag");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "Tag",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_ProductId",
                table: "Tag",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Products_ProductId",
                table: "Tag",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Products_ProductId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_ProductId",
                table: "Tag");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Tag",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductId1",
                table: "Tag",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_ProductId1",
                table: "Tag",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Products_ProductId1",
                table: "Tag",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "ProductId");
        }
    }
}

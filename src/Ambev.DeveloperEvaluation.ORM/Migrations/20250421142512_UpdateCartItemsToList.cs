using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCartItemsToList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("da670dbe-d189-4bda-b574-1f61a88a0a7d"));

            migrationBuilder.DropColumn(
                name: "Items",
                table: "Carts");

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CartId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Phone", "Role", "Status", "Username" },
                values: new object[] { new Guid("2f0a1971-bb71-4924-8a68-1fc9b4cda766"), "teste@ambev.com", "Admin", "Inicial", "$2a$11$FVPb6l3Awvycgn.T.E5MveW42hRECLIPZWo3AsKhRK7Lb1LVnXVWy", "999999999", "Admin", "Active", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("2f0a1971-bb71-4924-8a68-1fc9b4cda766"));

            migrationBuilder.AddColumn<int>(
                name: "Items",
                table: "Carts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Phone", "Role", "Status", "Username" },
                values: new object[] { new Guid("da670dbe-d189-4bda-b574-1f61a88a0a7d"), "teste@ambev.com", "Admin", "Inicial", "$2a$11$zTirqJ6F9J12Ei5OyRUmb.ZWubuwuMgGuXw1DZJvdi/ipBlmO2Y6S", "999999999", "Admin", "Active", "admin" });
        }
    }
}

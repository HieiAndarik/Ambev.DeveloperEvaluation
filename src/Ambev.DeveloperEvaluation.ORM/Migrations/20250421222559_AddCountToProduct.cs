using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class AddCountToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("2f0a1971-bb71-4924-8a68-1fc9b4cda766"));

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Rate",
                table: "Products",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Phone", "Role", "Status", "Username" },
                values: new object[] { new Guid("eea3ebf9-2ff6-4670-81ae-fcc22477ef96"), "teste@ambev.com", "Admin", "Inicial", "$2a$11$QM.3nrSDB755YMUHyw90s.EwvwbYDRXXtgiG/JKTx2cQ58cRjbNca", "999999999", "Admin", "Active", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("eea3ebf9-2ff6-4670-81ae-fcc22477ef96"));

            migrationBuilder.DropColumn(
                name: "Count",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Products");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Phone", "Role", "Status", "Username" },
                values: new object[] { new Guid("2f0a1971-bb71-4924-8a68-1fc9b4cda766"), "teste@ambev.com", "Admin", "Inicial", "$2a$11$FVPb6l3Awvycgn.T.E5MveW42hRECLIPZWo3AsKhRK7Lb1LVnXVWy", "999999999", "Admin", "Active", "admin" });
        }
    }
}

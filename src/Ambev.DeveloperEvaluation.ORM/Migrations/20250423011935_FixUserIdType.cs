using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class FixUserIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("eea3ebf9-2ff6-4670-81ae-fcc22477ef96"));

            migrationBuilder.Sql("DELETE FROM \"Carts\";");
            migrationBuilder.Sql("ALTER TABLE \"Carts\" DROP COLUMN \"UserId\";");
            migrationBuilder.Sql("ALTER TABLE \"Carts\" ADD COLUMN \"UserId\" uuid NOT NULL;");


            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Phone", "Role", "Status", "Username" },
                values: new object[] { new Guid("ba0fec5f-3868-40c7-a015-f57bd0a45a30"), "teste@ambev.com", "Admin", "Inicial", "$2a$11$Kozzkt7i5nR77CZqZ0DIler16HQb4rg5U9jqa.8mxiv/9w090OVdi", "999999999", "Admin", "Active", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ba0fec5f-3868-40c7-a015-f57bd0a45a30"));

            migrationBuilder.Sql("ALTER TABLE \"Carts\" DROP COLUMN \"UserId\";");
            migrationBuilder.Sql("ALTER TABLE \"Carts\" ADD COLUMN \"UserId\" integer NOT NULL;");


            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Phone", "Role", "Status", "Username" },
                values: new object[] { new Guid("eea3ebf9-2ff6-4670-81ae-fcc22477ef96"), "teste@ambev.com", "Admin", "Inicial", "$2a$11$QM.3nrSDB755YMUHyw90s.EwvwbYDRXXtgiG/JKTx2cQ58cRjbNca", "999999999", "Admin", "Active", "admin" });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Phone", "Role", "Status", "Username" },
                values: new object[] { new Guid("da670dbe-d189-4bda-b574-1f61a88a0a7d"), "teste@ambev.com", "Admin", "Inicial", "$2a$11$zTirqJ6F9J12Ei5OyRUmb.ZWubuwuMgGuXw1DZJvdi/ipBlmO2Y6S", "999999999", "Admin", "Active", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("da670dbe-d189-4bda-b574-1f61a88a0a7d"));
        }
    }
}

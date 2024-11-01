using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerse_shoes_backend.Migrations
{
    /// <inheritdoc />
    public partial class admin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Isblocked", "Name", "Password", "Phone_no", "Role" },
                values: new object[] { 10, "admin@gmail.com", false, "admin", "$2a$11$RxUgYKz1V0pM/HXrlivitu29DnUgN0QkMjMSs5cnC0vYitnJMmqoW", 9087675434L, "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_Registration_System.Migrations
{
    /// <inheritdoc />
    public partial class Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    userEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    userProfile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    registerDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

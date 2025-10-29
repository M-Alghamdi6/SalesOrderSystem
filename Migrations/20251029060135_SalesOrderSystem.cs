using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesOrderSystem_BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class SalesOrderSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "apps");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "apps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesRequestNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SalesDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SalesNote = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Approver = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RejectionRemark = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ApprovalRemark = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RequesterUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesRequest_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "apps",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesRequestLine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesRequestId = table.Column<int>(type: "int", nullable: false),
                    LineNo = table.Column<int>(type: "int", nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ItemDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ItemUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesRequestLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesRequestLine_SalesRequest_SalesRequestId",
                        column: x => x.SalesRequestId,
                        principalTable: "SalesRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesRequest_UserId",
                table: "SalesRequest",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesRequestLine_SalesRequestId",
                table: "SalesRequestLine",
                column: "SalesRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesRequestLine");

            migrationBuilder.DropTable(
                name: "SalesRequest");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "apps");
        }
    }
}

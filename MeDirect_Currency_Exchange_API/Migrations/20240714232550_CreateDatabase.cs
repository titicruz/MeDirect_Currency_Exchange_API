using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeDirect_Currency_Exchange_API.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    DT_Create = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("IDX_Client_ID", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Trade",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ID_Client = table.Column<int>(type: "INTEGER", nullable: false),
                    FromCurrency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    ToCurrency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    ExchangedAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Dt_Create = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("IDX_Trade_ID", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Client_Trades_ID_Client",
                        column: x => x.ID_Client,
                        principalTable: "Client",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_Trade_ID_Client",
                table: "Trade",
                column: "ID_Client");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trade");

            migrationBuilder.DropTable(
                name: "Client");
        }
    }
}

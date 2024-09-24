using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApplication.Dal.Migrations
{
    /// <inheritdoc />
    public partial class Requesttableupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "RequestStatus",
                table: "Requests",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "SupplierTaxId",
                table: "Requests",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestStatus",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SupplierTaxId",
                table: "Requests");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QassimPay.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAddressModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Address",
                newName: "ADD_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ADD_ID",
                table: "Address",
                newName: "ID");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QassimPay.Migrations
{
    /// <inheritdoc />
    public partial class updatedbconect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_User_U_ID",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_User_User_ID",
                table: "Wallet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "USER");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "T_date",
                table: "Transfer",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_USER",
                table: "USER",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_USER_U_ID",
                table: "Address",
                column: "U_ID",
                principalTable: "USER",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_USER_User_ID",
                table: "Wallet",
                column: "User_ID",
                principalTable: "USER",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_USER_U_ID",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_USER_User_ID",
                table: "Wallet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_USER",
                table: "USER");

            migrationBuilder.RenameTable(
                name: "USER",
                newName: "User");

            migrationBuilder.AlterColumn<DateTime>(
                name: "T_date",
                table: "Transfer",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_User_U_ID",
                table: "Address",
                column: "U_ID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_User_User_ID",
                table: "Wallet",
                column: "User_ID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

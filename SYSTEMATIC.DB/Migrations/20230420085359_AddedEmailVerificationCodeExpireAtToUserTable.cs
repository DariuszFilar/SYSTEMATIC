using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SYSTEMATIC.DB.Migrations
{
    public partial class AddedEmailVerificationCodeExpireAtToUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EmailVerificationCodeExpireAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailVerificationCodeExpireAt",
                table: "Users");
        }
    }
}

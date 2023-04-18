using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SYSTEMATIC.DB.Migrations
{
    public partial class AddedEmailVerificationCodeInUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailVerificationCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailVerificationCode",
                table: "Users");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;


namespace SYSTEMATIC.DB.Migrations
{
    public partial class AddedEmailVerificationCodeInUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<string>(
                name: "EmailVerificationCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "EmailVerificationCode",
                table: "Users");
        }
    }
}

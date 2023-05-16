using Microsoft.EntityFrameworkCore.Migrations;


namespace SYSTEMATIC.DB.Migrations
{
    public partial class AddedEmailVerificationCodeSendedAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<DateTime>(
                name: "EmialVerificationCodeSendedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "EmialVerificationCodeSendedAt",
                table: "Users");
        }
    }
}

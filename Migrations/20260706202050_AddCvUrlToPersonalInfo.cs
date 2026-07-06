using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebPage.Migrations
{
    /// <inheritdoc />
    public partial class AddCvUrlToPersonalInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CvUrl",
                table: "PersonalInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PersonalInfos",
                keyColumn: "Id",
                keyValue: 1,
                column: "CvUrl",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CvUrl",
                table: "PersonalInfos");
        }
    }
}

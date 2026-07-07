using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebPage.Migrations
{
    /// <inheritdoc />
    public partial class MakeProjectFieldsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE Projects ALTER COLUMN LiveLink nvarchar(max) NULL;");
            migrationBuilder.Sql("ALTER TABLE Projects ALTER COLUMN ImageUrl nvarchar(max) NULL;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Note: Down migration is destructive if it truncates data, so we leave it or revert.
            migrationBuilder.Sql("ALTER TABLE Projects ALTER COLUMN LiveLink nvarchar(max) NOT NULL;");
            migrationBuilder.Sql("ALTER TABLE Projects ALTER COLUMN ImageUrl nvarchar(max) NOT NULL;");
        }
    }
}

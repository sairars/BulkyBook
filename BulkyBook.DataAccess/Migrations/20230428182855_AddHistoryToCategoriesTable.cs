using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulkyBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddHistoryToCategoriesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var dateTime = DateTime.Now;

            migrationBuilder.Sql(@$"INSERT INTO Categories(Name, DisplayOrder, CreatedDateTime) VALUES('History', 6, '{dateTime}')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM dbo.Categories WHERE Name IN ('History')");
        }
    }
}

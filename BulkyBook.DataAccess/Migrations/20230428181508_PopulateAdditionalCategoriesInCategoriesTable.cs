using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulkyBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class PopulateAdditionalCategoriesInCategoriesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var dateTime = DateTime.Now;

            migrationBuilder.Sql(@$"INSERT INTO Categories(Name, DisplayOrder, CreatedDateTime) VALUES('SciFi', 5, '{dateTime}')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM dbo.Categories WHERE Name IN ('SciFi')");
        }
    }
}

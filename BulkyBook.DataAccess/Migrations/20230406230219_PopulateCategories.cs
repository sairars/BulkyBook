using Microsoft.EntityFrameworkCore.Migrations;


#nullable disable

namespace BulkyBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class PopulateCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var dateTime = DateTime.Now;
            var name = "Action";
            migrationBuilder.Sql(@$"INSERT INTO Categories(Name, DisplayOrder, CreatedDateTime) VALUES('{name}', 1, '{dateTime}')");
            //migrationBuilder.Sql(@$"INSERT INTO dbo.Categories(Name, DisplayOrder, CreatedDateTime) VALUES('Fiction', 2, {dateTime});");
            //migrationBuilder.Sql(@$"INSERT INTO dbo.Categories(Name, DisplayOrder, CreatedDateTime) VALUES('Romance', 3, {dateTime});");
            //migrationBuilder.Sql(@$"INSERT INTO dbo.Categories(Name, DisplayOrder, CreatedDateTime) VALUES('Fantasy', 4, {dateTime});");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var name = "Action";
            migrationBuilder.Sql($"DELETE FROM dbo.Categories WHERE Name IN ('{name}')");
        }
    }
}

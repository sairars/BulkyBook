using BulkyBook.Utility;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;

#nullable disable

namespace BulkyBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class PopulateCoverTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"INSERT INTO CoverTypes(Name) 
                                    VALUES('Hard Cover')");
            migrationBuilder.Sql(@$"INSERT INTO CoverTypes(Name) 
                                    VALUES('Paper Back')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"DELETE FROM CoverTypes 
                                    WHERE Name IN ('Hard Cover', 
                                                    'Paper Back')");
        }
    }
}

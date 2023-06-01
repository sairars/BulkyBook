using BulkyBook.Utility;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;

#nullable disable

namespace BulkyBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class PopulateAspNetRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"INSERT INTO AspNetRoles(Id, Name, NormalizedName, ConcurrencyStamp) 
                                    VALUES('{Guid.NewGuid()}', 
                                            '{StaticDetails.Admin}', 
                                            '{StaticDetails.Admin.ToUpperInvariant()}', 
                                            '{Guid.NewGuid()}')");
            migrationBuilder.Sql(@$"INSERT INTO AspNetRoles(Id, Name, NormalizedName, ConcurrencyStamp) 
                                    VALUES('{Guid.NewGuid()}', 
                                            '{StaticDetails.Staff}', 
                                            '{StaticDetails.Staff.ToUpperInvariant()}', 
                                            '{Guid.NewGuid()}')");
            migrationBuilder.Sql(@$"INSERT INTO AspNetRoles(Id, Name, NormalizedName, ConcurrencyStamp) 
                                    VALUES('{Guid.NewGuid()}', 
                                            '{StaticDetails.IndividualUser}', 
                                            '{StaticDetails.IndividualUser.ToUpperInvariant()}', 
                                            '{Guid.NewGuid()}')");
            migrationBuilder.Sql(@$"INSERT INTO AspNetRoles(Id, Name, NormalizedName, ConcurrencyStamp) 
                                    VALUES('{Guid.NewGuid()}', 
                                            '{StaticDetails.CompanyUser}', 
                                            '{StaticDetails.CompanyUser.ToUpperInvariant()}', 
                                            '{Guid.NewGuid()}')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"DELETE FROM AspNetRoles 
                                    WHERE Name IN ('{StaticDetails.Admin}', 
                                                    '{StaticDetails.Staff}', 
                                                    '{StaticDetails.IndividualUser}', 
                                                    '{StaticDetails.CompanyUser}')");
        }
    }
}

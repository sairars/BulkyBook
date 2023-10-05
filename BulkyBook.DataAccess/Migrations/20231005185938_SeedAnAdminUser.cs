using BulkyBook.Utility;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulkyBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedAnAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, 
                                                                EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, 
                                                                PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, 
                                                                LockoutEnd, LockoutEnabled, AccessFailedCount, 
                                                                City, Discriminator, Name, PostalCode, State, StreetAddress, 
                                                                CompanyId) 
                                    VALUES ('94424829-d049-4048-a9c0-74cd61d0b1cb', 'admin@bulky.com', 'ADMIN@BULKY.COM',
                                            'admin@bulky.com', 'ADMIN@BULKY.COM', 0, 
                                            'AQAAAAEAACcQAAAAENclhiGpljT+QXZiL6c3+1ZbAdLFedx7X5j2gqnR9QvGRVelTs1Nqa/pL7qLJpcuAw==',
                                            'QDM3VSYN4DUQ22D6YZM6AB2RNDBSWLJN', 'b339a65e-f713-48a5-9e90-c7705da63c35', 
                                            '617-877-9334', 0, 0, NULL, 1, 0, 'Barrie', 'ApplicationUser', 
                                            'Bulky Admin', '78345', 'MI', '89 Rogerson Drive', NULL)");

            migrationBuilder.Sql(@$"INSERT INTO AspNetUserRoles(UserId, RoleId) 
                                    SELECT '94424829-d049-4048-a9c0-74cd61d0b1cb', Id
                                    FROM AspNetRoles 
                                    WHERE Name = '{StaticDetails.Admin}'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"DELETE FROM AspNetUserRoles 
                                    WHERE UserId = '94424829-d049-4048-a9c0-74cd61d0b1cb' 
                                        AND RoleId = 'd22efe36-97ba-4574-ae76-862d7fb0d3f1'");

            migrationBuilder.Sql(@$"DELETE FROM AspNetUsers 
                                    WHERE Id = '94424829-d049-4048-a9c0-74cd61d0b1cb'");
        }
    }
}

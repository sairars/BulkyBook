using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulkyBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
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
                                    VALUES ('ebc42e34-5590-45d6-8a90-8fa0724c3ee3', 'admin1@bulky.com', 'ADMIN1@BULKY.COM',
                                            'admin1@bulky.com', 'ADMIN1@BULKY.COM', 0, 
                                            'AQAAAAEAACcQAAAAEE2tBeyD+zbbov/xXKUd4DOH0TKXtLItUxzsPabwZFhibOVUTzgJA5rSuIZqSEaP/A==',
                                            'NPRPXEWWORBV3E74E4Q2MSZGRXBR4PQV', '84188825-c7c8-4792-8adb-6f826fc62cff', 
                                            '617-877-9334', 0, 0, NULL, 1, 0, 'Barrie', 'ApplicationUser', 
                                            'Admin 1', '78345', 'MI', '89 Rogerson Drive', NULL)");

            migrationBuilder.Sql(@$"INSERT INTO AspNetUserRoles(UserId, RoleId) 
                                    VALUES ('ebc42e34-5590-45d6-8a90-8fa0724c3ee3', 
                                            'd22efe36-97ba-4574-ae76-862d7fb0d3f1')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"DELETE FROM AspNetUserRoles 
                                    WHERE UserId = 'ebc42e34-5590-45d6-8a90-8fa0724c3ee3' 
                                        AND RoleId = 'd22efe36-97ba-4574-ae76-862d7fb0d3f1'");

            migrationBuilder.Sql(@$"DELETE FROM AspNetUsers 
                                    WHERE Id = 'ebc42e34-5590-45d6-8a90-8fa0724c3ee3'");
        }
    }
}

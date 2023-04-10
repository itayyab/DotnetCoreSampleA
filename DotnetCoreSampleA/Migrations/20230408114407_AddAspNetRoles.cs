using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetCoreSampleA.Migrations
{
    public partial class AddAspNetRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES ('869a64b8-d601-11ed-afa1-0242ac120002', 'ADMIN', 'ADMIN', null)");
            migrationBuilder.Sql("INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES ('869a680a-d601-11ed-afa1-0242ac120002', 'CUSTOMER', 'CUSTOMER', null)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [AspNetRoles] WHERE [Id] = '869a64b8-d601-11ed-afa1-0242ac120002'");
            migrationBuilder.Sql("DELETE FROM [AspNetRoles] WHERE [Id] = '869a680a-d601-11ed-afa1-0242ac120002'");
        }
    }
}

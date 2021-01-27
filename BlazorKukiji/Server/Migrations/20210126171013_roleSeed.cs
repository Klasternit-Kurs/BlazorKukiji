using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorKukiji.Server.Migrations
{
    public partial class roleSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ba15490e-7f08-4286-a14f-ab8997ece6b1", "zxcvsdc", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ba15490e-7f08-4286-a14f-ab8997ece6b1");
        }
    }
}

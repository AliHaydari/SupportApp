using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SupportApp.DataLayer.Migrations
{
    public partial class AddSupportEndDateToCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SupportEndDate",
                table: "Customer",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupportEndDate",
                table: "Customer");
        }
    }
}

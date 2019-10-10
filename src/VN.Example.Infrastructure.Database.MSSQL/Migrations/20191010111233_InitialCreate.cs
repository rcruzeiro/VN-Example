using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VN.Example.Infrastructure.Database.MSSQL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Behaviors",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IP = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    UserAgent = table.Column<string>(nullable: false),
                    Parameters = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Behaviors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Behaviors_Id",
                table: "Behaviors",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Behaviors_IP_Name_UserAgent",
                table: "Behaviors",
                columns: new[] { "IP", "Name", "UserAgent" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Behaviors");
        }
    }
}

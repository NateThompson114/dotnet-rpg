using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnetrpgwebapi.Migrations
{
    /// <inheritdoc />
    public partial class AddMaxHealthColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxHitpoints",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxHitpoints",
                table: "Characters");
        }
    }
}

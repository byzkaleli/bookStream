using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookStream.Migrations
{
    /// <inheritdoc />
    public partial class RenameCoverImageToCoverImageNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BookGenres",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "BookGenres");
        }
    }
}

﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookStream.Migrations
{
    /// <inheritdoc />
    public partial class RenameCoverImageUrlToCoverImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE `Books` CHANGE COLUMN `CoverImageUrl` `CoverImage` VARCHAR(500);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE `Books` CHANGE COLUMN `CoverImage` `CoverImageUrl` VARCHAR(500);");
        }
    }
}

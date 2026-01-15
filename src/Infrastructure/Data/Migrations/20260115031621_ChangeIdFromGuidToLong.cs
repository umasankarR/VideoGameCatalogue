using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIdFromGuidToLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // WARNING: This will delete all existing data!
            // Step 1: Drop primary key
            migrationBuilder.DropPrimaryKey(
                name: "PK_VideoGames",
                table: "VideoGames");

            // Step 2: Drop old Id column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "VideoGames");

            // Step 3: Add new Id column as bigint with identity
            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "VideoGames",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            // Step 4: Add primary key constraint
            migrationBuilder.AddPrimaryKey(
                name: "PK_VideoGames",
                table: "VideoGames",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "VideoGames",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}

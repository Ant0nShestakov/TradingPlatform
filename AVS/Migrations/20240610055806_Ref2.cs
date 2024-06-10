using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AVS.Migrations
{
    /// <inheritdoc />
    public partial class Ref2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Advertisements_AdvertisementId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_AdvertisementId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "AdvertisementId",
                table: "Messages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AdvertisementId",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AdvertisementId",
                table: "Messages",
                column: "AdvertisementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Advertisements_AdvertisementId",
                table: "Messages",
                column: "AdvertisementId",
                principalTable: "Advertisements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

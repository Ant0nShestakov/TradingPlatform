using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AVS.Migrations
{
    /// <inheritdoc />
    public partial class fixUniqueOnStreetName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Streets_Name",
                table: "Streets");

            migrationBuilder.CreateIndex(
                name: "IX_Streets_Name",
                table: "Streets",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Streets_Name",
                table: "Streets");

            migrationBuilder.CreateIndex(
                name: "IX_Streets_Name",
                table: "Streets",
                column: "Name",
                unique: true);
        }
    }
}

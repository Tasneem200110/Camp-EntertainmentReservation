using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class image : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Camps_Addresses_AddressId",
                table: "Camps");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Camps",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CampId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Camps_CampId",
                        column: x => x.CampId,
                        principalTable: "Camps",
                        principalColumn: "CampID");
                    table.ForeignKey(
                        name: "FK_Images_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_CampId",
                table: "Images",
                column: "CampId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_UserId",
                table: "Images",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Camps_Addresses_AddressId",
                table: "Camps",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Camps_Addresses_AddressId",
                table: "Camps");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Camps",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Camps_Addresses_AddressId",
                table: "Camps",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DCXAir.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateTablesBackend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Journeys_JourneyId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Transport_TransportId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_JourneyId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_TransportId",
                table: "Flights");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transport",
                table: "Transport");

            migrationBuilder.DropColumn(
                name: "JourneyId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "TransportId",
                table: "Flights");

            migrationBuilder.RenameTable(
                name: "Transport",
                newName: "Transports");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Flights",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Transports",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transports",
                table: "Transports",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Journeys_Id",
                table: "Flights",
                column: "Id",
                principalTable: "Journeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transports_Flights_Id",
                table: "Transports",
                column: "Id",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Journeys_Id",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Transports_Flights_Id",
                table: "Transports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transports",
                table: "Transports");

            migrationBuilder.RenameTable(
                name: "Transports",
                newName: "Transport");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Flights",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "JourneyId",
                table: "Flights",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransportId",
                table: "Flights",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Transport",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transport",
                table: "Transport",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_JourneyId",
                table: "Flights",
                column: "JourneyId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_TransportId",
                table: "Flights",
                column: "TransportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Journeys_JourneyId",
                table: "Flights",
                column: "JourneyId",
                principalTable: "Journeys",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Transport_TransportId",
                table: "Flights",
                column: "TransportId",
                principalTable: "Transport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

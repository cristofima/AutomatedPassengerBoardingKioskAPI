using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutomatedPassengerBoardingKiosk.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Number = table.Column<int>(type: "int", nullable: false),
                    Carrier = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    From = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    To = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BoardingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gate = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Number);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    DocumentNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.DocumentNumber);
                });

            migrationBuilder.CreateTable(
                name: "BoardingPasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PassengerDocumentNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    FlightNumber = table.Column<int>(type: "int", nullable: false),
                    Seat = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Class = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    NameValidation = table.Column<bool>(type: "bit", nullable: false),
                    DobValidation = table.Column<bool>(type: "bit", nullable: false),
                    FaceValidation = table.Column<bool>(type: "bit", nullable: false),
                    BoardingPassValidation = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardingPasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardingPasses_Flights_FlightNumber",
                        column: x => x.FlightNumber,
                        principalTable: "Flights",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardingPasses_People_PassengerDocumentNumber",
                        column: x => x.PassengerDocumentNumber,
                        principalTable: "People",
                        principalColumn: "DocumentNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardingPasses_FlightNumber",
                table: "BoardingPasses",
                column: "FlightNumber");

            migrationBuilder.CreateIndex(
                name: "IX_BoardingPasses_PassengerDocumentNumber",
                table: "BoardingPasses",
                column: "PassengerDocumentNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardingPasses");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "People");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceDay.API.CSharp.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentDocumentAndWeatherTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventDocuments",
                columns: table => new
                {
                    DocumentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventID = table.Column<int>(type: "int", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BlobFileName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UploadedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventDocuments", x => x.DocumentID);
                    table.ForeignKey(
                        name: "FK_EventDocuments_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventID");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnrolmentID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TransactionReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_Payments_Enrolments_EnrolmentID",
                        column: x => x.EnrolmentID,
                        principalTable: "Enrolments",
                        principalColumn: "EnrolmentID");
                });

            migrationBuilder.CreateTable(
                name: "WeatherInformation",
                columns: table => new
                {
                    WeatherID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventID = table.Column<int>(type: "int", nullable: false),
                    ForecastDate = table.Column<DateTime>(type: "date", nullable: false),
                    TemperatureC = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Condition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WindSpeedKmh = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    RainProbability = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    RetrievedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherInformation", x => x.WeatherID);
                    table.ForeignKey(
                        name: "FK_WeatherInformation_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventDocuments_EventID",
                table: "EventDocuments",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_EnrolmentID",
                table: "Payments",
                column: "EnrolmentID");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherInformation_EventID",
                table: "WeatherInformation",
                column: "EventID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventDocuments");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "WeatherInformation");
        }
    }
}

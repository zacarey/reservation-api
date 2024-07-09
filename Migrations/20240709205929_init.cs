using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reservation_api.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "client",
                columns: table => new
                {
                    client_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client", x => x.client_id);
                });

            migrationBuilder.CreateTable(
                name: "provider",
                columns: table => new
                {
                    provider_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provider", x => x.provider_id);
                });

            migrationBuilder.CreateTable(
                name: "appointment",
                columns: table => new
                {
                    appointment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider_id = table.Column<Guid>(type: "uuid", nullable: false),
                    timeslot = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    reserved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    reserved_by_client_id = table.Column<Guid>(type: "uuid", nullable: true),
                    confirmed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointment", x => x.appointment_id);
                    table.ForeignKey(
                        name: "FK_appointment_client_reserved_by_client_id",
                        column: x => x.reserved_by_client_id,
                        principalTable: "client",
                        principalColumn: "client_id");
                    table.ForeignKey(
                        name: "FK_appointment_provider_provider_id",
                        column: x => x.provider_id,
                        principalTable: "provider",
                        principalColumn: "provider_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_appointment_provider_id",
                table: "appointment",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_reserved_by_client_id",
                table: "appointment",
                column: "reserved_by_client_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appointment");

            migrationBuilder.DropTable(
                name: "client");

            migrationBuilder.DropTable(
                name: "provider");
        }
    }
}

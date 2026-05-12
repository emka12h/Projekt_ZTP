using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuestService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WeddingId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    RsvpStatus = table.Column<string>(type: "text", nullable: false),
                    InvitationSentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RsvpRespondedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Group = table.Column<string>(type: "text", nullable: false),
                    Side = table.Column<string>(type: "text", nullable: false),
                    IsVip = table.Column<bool>(type: "boolean", nullable: false),
                    HasPlusOne = table.Column<bool>(type: "boolean", nullable: false),
                    PlusOneName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TableNumber = table.Column<int>(type: "integer", nullable: true),
                    SeatNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    MealPreference = table.Column<string>(type: "text", nullable: false),
                    Allergies = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    NeedsAccommodation = table.Column<bool>(type: "boolean", nullable: false),
                    NeedsTransport = table.Column<bool>(type: "boolean", nullable: false),
                    CheckedIn = table.Column<bool>(type: "boolean", nullable: false),
                    CheckedInAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guests", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Guests_Email",
                table: "Guests",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Guests_WeddingId",
                table: "Guests",
                column: "WeddingId");

            migrationBuilder.CreateIndex(
                name: "IX_Guests_WeddingId_RsvpStatus",
                table: "Guests",
                columns: new[] { "WeddingId", "RsvpStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Guests_WeddingId_TableNumber",
                table: "Guests",
                columns: new[] { "WeddingId", "TableNumber" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Guests");
        }
    }
}

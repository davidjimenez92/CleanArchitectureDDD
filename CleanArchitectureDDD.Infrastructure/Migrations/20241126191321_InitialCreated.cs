using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitectureDDD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    last_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    model = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    vin = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: true),
                    address_street = table.Column<string>(type: "text", nullable: true),
                    address_city = table.Column<string>(type: "text", nullable: true),
                    address_state = table.Column<string>(type: "text", nullable: true),
                    address_zip_code = table.Column<string>(type: "text", nullable: true),
                    price_price = table.Column<decimal>(type: "numeric", nullable: true),
                    price_currency_type = table.Column<string>(type: "text", nullable: true),
                    maintenance_price = table.Column<decimal>(type: "numeric", nullable: true),
                    maintenance_currency_type = table.Column<string>(type: "text", nullable: true),
                    last_rent = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    accessories = table.Column<int[]>(type: "integer[]", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    duration_start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    duration_end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    vehicle_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    price_price = table.Column<decimal>(type: "numeric", nullable: true),
                    price_currency_type = table.Column<string>(type: "text", nullable: true),
                    maintenance_price = table.Column<decimal>(type: "numeric", nullable: true),
                    maintenance_currency_type = table.Column<string>(type: "text", nullable: true),
                    accessories_price_price = table.Column<decimal>(type: "numeric", nullable: true),
                    accessories_price_currency_type = table.Column<string>(type: "text", nullable: true),
                    total_price_price = table.Column<decimal>(type: "numeric", nullable: true),
                    total_price_currency_type = table.Column<string>(type: "text", nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    date_confirmation = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    date_rejection = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    date_copletation = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    date_cancellation = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rents", x => x.id);
                    table.ForeignKey(
                        name: "fk_rents_user_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_rents_vehicle_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    vehicle_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rent_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reviews", x => x.id);
                    table.ForeignKey(
                        name: "fk_reviews_rents_rent_id",
                        column: x => x.rent_id,
                        principalTable: "rents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_reviews_user_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_reviews_vehicle_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_rents_user_id",
                table: "rents",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_rents_vehicle_id",
                table: "rents",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_rent_id",
                table: "reviews",
                column: "rent_id");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_user_id",
                table: "reviews",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_vehicle_id",
                table: "reviews",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "rents");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "vehicles");
        }
    }
}

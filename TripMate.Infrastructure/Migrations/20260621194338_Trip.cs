using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TripMate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Trip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    flight_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    origin_airport = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    destination_airport = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    airline_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    flight_number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    departure_datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    arrival_datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    duration_minutes = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    cabin_class = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    seats_available = table.Column<int>(type: "int", nullable: true),
                    baggage_policy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Flights__E3705765DA77E091", x => x.flight_id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    ItemsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Roles__760965CCFF80CC4C", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "TripTypes",
                columns: table => new
                {
                    trip_type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TripType__74B29FC1BF55D401", x => x.trip_type_id);
                });

            migrationBuilder.CreateTable(
                name: "UserInteractions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DestinationId = table.Column<int>(type: "int", nullable: false),
                    InteractionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInteractions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Otp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    password_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    profile_image_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    preferred_language = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    preferred_currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    email_verified = table.Column<bool>(type: "bit", nullable: false),
                    phone_verified = table.Column<bool>(type: "bit", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__B9BE370FB16FDC5D", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_Users_Roles",
                        column: x => x.role_id,
                        principalTable: "Roles",
                        principalColumn: "role_id");
                });

            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    destination_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    trip_type_id = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    airport_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DurationDays = table.Column<int>(type: "int", nullable: false),
                    Itinerary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    main_image_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    average_rating = table.Column<decimal>(type: "decimal(3,2)", nullable: true),
                    min_price_estimate = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    timezone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    latitude = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    longitude = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Destinat__55015391DD0D892B", x => x.destination_id);
                    table.ForeignKey(
                        name: "FK_Destinations_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Destinations_TripTypes",
                        column: x => x.trip_type_id,
                        principalTable: "TripTypes",
                        principalColumn: "trip_type_id");
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    favorite_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    item_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Favorite__46ACF4CBA1E9B777", x => x.favorite_id);
                    table.ForeignKey(
                        name: "FK_Favorites_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    notification_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    is_read = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notifica__E059842F9E802E30", x => x.notification_id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    rating_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    item_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    value = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.rating_id);
                    table.ForeignKey(
                        name: "FK_Ratings_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    review_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    entity_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    entity_id = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reviews__60883D905D1AAAAD", x => x.review_id);
                    table.ForeignKey(
                        name: "FK_Reviews_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "UserPreferences",
                columns: table => new
                {
                    user_pref_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    preferred_trip_type_id = table.Column<int>(type: "int", nullable: true),
                    min_budget = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    max_budget = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    preferred_season = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    preferred_airlines = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserPref__BDC9BDBE1BE3CCF6", x => x.user_pref_id);
                    table.ForeignKey(
                        name: "FK_UserPreferences_TripTypes",
                        column: x => x.preferred_trip_type_id,
                        principalTable: "TripTypes",
                        principalColumn: "trip_type_id");
                    table.ForeignKey(
                        name: "FK_UserPreferences_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DestinationMedia",
                columns: table => new
                {
                    media_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    destination_id = table.Column<int>(type: "int", nullable: false),
                    media_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    media_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Destinat__D0A840F466876063", x => x.media_id);
                    table.ForeignKey(
                        name: "FK_DestinationMedia_Destinations",
                        column: x => x.destination_id,
                        principalTable: "Destinations",
                        principalColumn: "destination_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    package_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    destination_id = table.Column<int>(type: "int", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    max_guests = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    short_description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    full_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    duration_days = table.Column<int>(type: "int", nullable: false),
                    base_price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    available_from = table.Column<DateOnly>(type: "date", nullable: true),
                    available_to = table.Column<DateOnly>(type: "date", nullable: true),
                    included_text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    excluded_text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    average_rating = table.Column<decimal>(type: "decimal(3,2)", nullable: true),
                    tags = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Packages__63846AE84877CC47", x => x.package_id);
                    table.ForeignKey(
                        name: "FK_Packages_Destinations",
                        column: x => x.destination_id,
                        principalTable: "Destinations",
                        principalColumn: "destination_id");
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    booking_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    package_id = table.Column<int>(type: "int", nullable: true),
                    destination_id = table.Column<int>(type: "int", nullable: true),
                    flight_id = table.Column<int>(type: "int", nullable: true),
                    number_of_people = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    payment_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    booking_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    total_price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    travel_start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    travel_end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    booking_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    booked_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Bookings__5DE3A5B1F0325959", x => x.booking_id);
                    table.ForeignKey(
                        name: "FK_Bookings_Destinations",
                        column: x => x.destination_id,
                        principalTable: "Destinations",
                        principalColumn: "destination_id");
                    table.ForeignKey(
                        name: "FK_Bookings_Flights",
                        column: x => x.flight_id,
                        principalTable: "Flights",
                        principalColumn: "flight_id");
                    table.ForeignKey(
                        name: "FK_Bookings_Packages",
                        column: x => x.package_id,
                        principalTable: "Packages",
                        principalColumn: "package_id");
                    table.ForeignKey(
                        name: "FK_Bookings_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "PackageFlights",
                columns: table => new
                {
                    package_flight_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    package_id = table.Column<int>(type: "int", nullable: false),
                    flight_id = table.Column<int>(type: "int", nullable: false),
                    flight_role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    price_difference = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PackageF__51D92FC365C61F08", x => x.package_flight_id);
                    table.ForeignKey(
                        name: "FK_PackageFlights_Flights",
                        column: x => x.flight_id,
                        principalTable: "Flights",
                        principalColumn: "flight_id");
                    table.ForeignKey(
                        name: "FK_PackageFlights_Packages",
                        column: x => x.package_id,
                        principalTable: "Packages",
                        principalColumn: "package_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackageMedia",
                columns: table => new
                {
                    media_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    package_id = table.Column<int>(type: "int", nullable: false),
                    media_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    media_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PackageM__D0A840F4725B70DD", x => x.media_id);
                    table.ForeignKey(
                        name: "FK_PackageMedia_Packages",
                        column: x => x.package_id,
                        principalTable: "Packages",
                        principalColumn: "package_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    post_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    location = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image_url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    is_public = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())"),
                    DestinationId = table.Column<int>(type: "int", nullable: true),
                    PackageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Posts__3ED7876658D7ED95", x => x.post_id);
                    table.ForeignKey(
                        name: "FK_Posts_Destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destinations",
                        principalColumn: "destination_id");
                    table.ForeignKey(
                        name: "FK_Posts_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "package_id");
                    table.ForeignKey(
                        name: "FK_Posts_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "RecommendationLogs",
                columns: table => new
                {
                    rec_log_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    destination_id = table.Column<int>(type: "int", nullable: true),
                    package_id = table.Column<int>(type: "int", nullable: true),
                    flight_id = table.Column<int>(type: "int", nullable: true),
                    algorithm_version = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    score = table.Column<decimal>(type: "decimal(5,3)", nullable: true),
                    shown_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())"),
                    clicked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Recommen__056DBA0666E7D7E4", x => x.rec_log_id);
                    table.ForeignKey(
                        name: "FK_RecommendationLogs_Destinations",
                        column: x => x.destination_id,
                        principalTable: "Destinations",
                        principalColumn: "destination_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecommendationLogs_Flights",
                        column: x => x.flight_id,
                        principalTable: "Flights",
                        principalColumn: "flight_id");
                    table.ForeignKey(
                        name: "FK_RecommendationLogs_Packages",
                        column: x => x.package_id,
                        principalTable: "Packages",
                        principalColumn: "package_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecommendationLogs_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserActions",
                columns: table => new
                {
                    action_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    destination_id = table.Column<int>(type: "int", nullable: true),
                    package_id = table.Column<int>(type: "int", nullable: true),
                    flight_id = table.Column<int>(type: "int", nullable: true),
                    action_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    action_value = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    action_timestamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserActi__74EFC217D72B3519", x => x.action_id);
                    table.ForeignKey(
                        name: "FK_UserActions_Destinations",
                        column: x => x.destination_id,
                        principalTable: "Destinations",
                        principalColumn: "destination_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserActions_Flights",
                        column: x => x.flight_id,
                        principalTable: "Flights",
                        principalColumn: "flight_id");
                    table.ForeignKey(
                        name: "FK_UserActions_Packages",
                        column: x => x.package_id,
                        principalTable: "Packages",
                        principalColumn: "package_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserActions_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    ItemType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingItems_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "booking_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingTravellers",
                columns: table => new
                {
                    traveler_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    booking_id = table.Column<int>(type: "int", nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    passport_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    contact_phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BookingT__53C729FA564ADB6C", x => x.traveler_id);
                    table.ForeignKey(
                        name: "FK_BookingTravellers_Bookings",
                        column: x => x.booking_id,
                        principalTable: "Bookings",
                        principalColumn: "booking_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    payment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    booking_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    gateway_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    transaction_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    transaction_reference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    paid_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payments__ED1FC9EA519FBE3F", x => x.payment_id);
                    table.ForeignKey(
                        name: "FK_Payments_Bookings",
                        column: x => x.booking_id,
                        principalTable: "Bookings",
                        principalColumn: "booking_id");
                });

            migrationBuilder.CreateTable(
                name: "PostMedia",
                columns: table => new
                {
                    media_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    post_id = table.Column<int>(type: "int", nullable: false),
                    media_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    media_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PostMedi__D0A840F4F9DE00FE", x => x.media_id);
                    table.ForeignKey(
                        name: "FK_PostMedia_Posts_post_id",
                        column: x => x.post_id,
                        principalTable: "Posts",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "role_id", "description", "name" },
                values: new object[,]
                {
                    { 1, null, "Admin" },
                    { 2, null, "Traveller" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingItems_BookingId",
                table: "BookingItems",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_destination_id",
                table: "Bookings",
                column: "destination_id");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_flight_id",
                table: "Bookings",
                column: "flight_id");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_package_id",
                table: "Bookings",
                column: "package_id");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_user_id",
                table: "Bookings",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Bookings__3A30D2BC44476419",
                table: "Bookings",
                column: "booking_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingTravellers_booking_id",
                table: "BookingTravellers",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_DestinationMedia_destination_id",
                table: "DestinationMedia",
                column: "destination_id");

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_CategoryId",
                table: "Destinations",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_trip_type_id",
                table: "Destinations",
                column: "trip_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_user_id",
                table: "Favorites",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_user_id",
                table: "Notifications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_PackageFlights_flight_id",
                table: "PackageFlights",
                column: "flight_id");

            migrationBuilder.CreateIndex(
                name: "IX_PackageFlights_package_id",
                table: "PackageFlights",
                column: "package_id");

            migrationBuilder.CreateIndex(
                name: "IX_PackageMedia_package_id",
                table: "PackageMedia",
                column: "package_id");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_destination_id",
                table: "Packages",
                column: "destination_id");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_booking_id",
                table: "Payments",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_PostMedia_post_id",
                table: "PostMedia",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_DestinationId",
                table: "Posts",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PackageId",
                table: "Posts",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_user_id",
                table: "Posts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_user_id",
                table: "Ratings",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationLogs_destination_id",
                table: "RecommendationLogs",
                column: "destination_id");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationLogs_flight_id",
                table: "RecommendationLogs",
                column: "flight_id");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationLogs_package_id",
                table: "RecommendationLogs",
                column: "package_id");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationLogs_user_id",
                table: "RecommendationLogs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_user_id",
                table: "Reviews",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserActions_destination_id",
                table: "UserActions",
                column: "destination_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserActions_flight_id",
                table: "UserActions",
                column: "flight_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserActions_package_id",
                table: "UserActions",
                column: "package_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserActions_user_id",
                table: "UserActions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_preferred_trip_type_id",
                table: "UserPreferences",
                column: "preferred_trip_type_id");

            migrationBuilder.CreateIndex(
                name: "UQ_UserPreferences_User",
                table: "UserPreferences",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_role_id",
                table: "Users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__AB6E6164DB183792",
                table: "Users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingItems");

            migrationBuilder.DropTable(
                name: "BookingTravellers");

            migrationBuilder.DropTable(
                name: "DestinationMedia");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PackageFlights");

            migrationBuilder.DropTable(
                name: "PackageMedia");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PaymentTransactions");

            migrationBuilder.DropTable(
                name: "PostMedia");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "RecommendationLogs");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "UserActions");

            migrationBuilder.DropTable(
                name: "UserInteractions");

            migrationBuilder.DropTable(
                name: "UserPreferences");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Destinations");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "TripTypes");
        }
    }
}

-- =========================================
-- ROLES
-- =========================================
CREATE TABLE Roles (
    role_id INT IDENTITY PRIMARY KEY,
    name NVARCHAR(50) NOT NULL,
    description NVARCHAR(255) NULL
);

-- =========================================
-- USERS
-- =========================================
CREATE TABLE Users (
    user_id INT IDENTITY PRIMARY KEY,
    role_id INT NOT NULL,
    full_name NVARCHAR(100) NOT NULL,
    email NVARCHAR(150) NOT NULL UNIQUE,
    phone NVARCHAR(30) NULL,
    password_hash NVARCHAR(255) NOT NULL,
    profile_image_url NVARCHAR(255) NULL,
    preferred_language NVARCHAR(10) NULL,
    preferred_currency NVARCHAR(10) NULL,
    email_verified BIT NOT NULL DEFAULT 0,
    phone_verified BIT NOT NULL DEFAULT 0,
    is_active BIT NOT NULL DEFAULT 1,
    created_at DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    updated_at DATETIME2 NULL,
    CONSTRAINT FK_Users_Roles
        FOREIGN KEY (role_id) REFERENCES Roles(role_id)
);

-- =========================================
-- TRIP_TYPES
-- =========================================
CREATE TABLE TripTypes (
    trip_type_id INT IDENTITY PRIMARY KEY,
    name NVARCHAR(50) NOT NULL,
    description NVARCHAR(255) NULL
);

-- =========================================
-- DESTINATIONS
-- =========================================
CREATE TABLE Destinations (
    destination_id INT IDENTITY PRIMARY KEY,
    trip_type_id INT NULL,
    name NVARCHAR(100) NOT NULL,
    country NVARCHAR(100) NOT NULL,
    city NVARCHAR(100) NULL,
    description NVARCHAR(MAX) NULL,
    main_image_url NVARCHAR(255) NULL,
    average_rating DECIMAL(3,2) NULL,
    min_price_estimate DECIMAL(10,2) NULL,
    timezone NVARCHAR(50) NULL,
    latitude DECIMAL(9,6) NULL,
    longitude DECIMAL(9,6) NULL,
    is_active BIT NOT NULL DEFAULT 1,
    created_at DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    updated_at DATETIME2 NULL,
    CONSTRAINT FK_Destinations_TripTypes
        FOREIGN KEY (trip_type_id) REFERENCES TripTypes(trip_type_id)
);

-- =========================================
-- DESTINATION_MEDIA
-- =========================================
CREATE TABLE DestinationMedia (
    media_id INT IDENTITY PRIMARY KEY,
    destination_id INT NOT NULL,
    media_url NVARCHAR(255) NOT NULL,
    media_type NVARCHAR(50) NOT NULL,
    sort_order INT NOT NULL DEFAULT 0,
    CONSTRAINT FK_DestinationMedia_Destinations
        FOREIGN KEY (destination_id) REFERENCES Destinations(destination_id)
        ON DELETE CASCADE
);

-- =========================================
-- PACKAGES
-- =========================================
CREATE TABLE Packages (
    package_id INT IDENTITY PRIMARY KEY,
    destination_id INT NOT NULL,
    name NVARCHAR(150) NOT NULL,
    short_description NVARCHAR(255) NULL,
    full_description NVARCHAR(MAX) NULL,
    duration_days INT NOT NULL,
    base_price DECIMAL(10,2) NOT NULL,
    currency NVARCHAR(10) NOT NULL,
    available_from DATE NULL,
    available_to DATE NULL,
    included_text NVARCHAR(MAX) NULL,
    excluded_text NVARCHAR(MAX) NULL,
    average_rating DECIMAL(3,2) NULL,
    tags NVARCHAR(255) NULL,
    is_active BIT NOT NULL DEFAULT 1,
    created_at DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    updated_at DATETIME2 NULL,
    CONSTRAINT FK_Packages_Destinations
        FOREIGN KEY (destination_id) REFERENCES Destinations(destination_id)
);

-- =========================================
-- PACKAGE_MEDIA
-- =========================================
CREATE TABLE PackageMedia (
    media_id INT IDENTITY PRIMARY KEY,
    package_id INT NOT NULL,
    media_url NVARCHAR(255) NOT NULL,
    media_type NVARCHAR(50) NOT NULL,
    sort_order INT NOT NULL DEFAULT 0,
    CONSTRAINT FK_PackageMedia_Packages
        FOREIGN KEY (package_id) REFERENCES Packages(package_id)
        ON DELETE CASCADE
);

-- =========================================
-- FLIGHTS
-- =========================================
CREATE TABLE Flights (
    flight_id INT IDENTITY PRIMARY KEY,
    origin_airport NVARCHAR(100) NOT NULL,
    destination_airport NVARCHAR(100) NOT NULL,
    airline_name NVARCHAR(100) NOT NULL,
    flight_number NVARCHAR(20) NOT NULL,
    departure_datetime DATETIME2 NOT NULL,
    arrival_datetime DATETIME2 NOT NULL,
    duration_minutes INT NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    currency NVARCHAR(10) NOT NULL,
    cabin_class NVARCHAR(20) NULL,
    seats_available INT NULL,
    baggage_policy NVARCHAR(255) NULL,
    is_active BIT NOT NULL DEFAULT 1,
    created_at DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    updated_at DATETIME2 NULL
);

-- =========================================
-- PACKAGE_FLIGHTS  (M:N between Packages & Flights)
-- =========================================
CREATE TABLE PackageFlights (
    package_flight_id INT IDENTITY PRIMARY KEY,
    package_id INT NOT NULL,
    flight_id INT NOT NULL,
    flight_role NVARCHAR(20) NOT NULL, -- outbound / return
    price_difference DECIMAL(10,2) NULL,
    CONSTRAINT FK_PackageFlights_Packages
        FOREIGN KEY (package_id) REFERENCES Packages(package_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_PackageFlights_Flights
        FOREIGN KEY (flight_id) REFERENCES Flights(flight_id)
);

-- =========================================
-- USER_PREFERENCES (1:1 with Users + UNIQUE)
-- =========================================
CREATE TABLE UserPreferences (
    user_pref_id INT IDENTITY PRIMARY KEY,
    user_id INT NOT NULL,
    preferred_trip_type_id INT NULL,
    min_budget DECIMAL(10,2) NULL,
    max_budget DECIMAL(10,2) NULL,
    preferred_season NVARCHAR(50) NULL,
    preferred_airlines NVARCHAR(255) NULL,
    created_at DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    updated_at DATETIME2 NULL,
    CONSTRAINT FK_UserPreferences_Users
        FOREIGN KEY (user_id) REFERENCES Users(user_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_UserPreferences_TripTypes
        FOREIGN KEY (preferred_trip_type_id) REFERENCES TripTypes(trip_type_id),
    CONSTRAINT UQ_UserPreferences_User
        UNIQUE (user_id)
);

-- =========================================
-- BOOKINGS
-- =========================================
CREATE TABLE Bookings (
    booking_id INT IDENTITY PRIMARY KEY,
    user_id INT NOT NULL,
    package_id INT NULL,
    destination_id INT NULL,  -- ممكن تبقى NULL لو booking طيران بس
    flight_id INT NULL,
    booking_type NVARCHAR(30) NOT NULL,    -- package / flight / both
    currency NVARCHAR(10) NOT NULL,
    total_price DECIMAL(10,2) NOT NULL,
    travel_start_date DATE NOT NULL,
    travel_end_date DATE NOT NULL,
    status NVARCHAR(20) NOT NULL,          -- pending/confirmed/cancelled/completed
    payment_status NVARCHAR(20) NOT NULL,  -- unpaid/paid/refunded
    booking_number NVARCHAR(50) NOT NULL UNIQUE,
    booked_at DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_Bookings_Users
        FOREIGN KEY (user_id) REFERENCES Users(user_id),
    CONSTRAINT FK_Bookings_Packages
        FOREIGN KEY (package_id) REFERENCES Packages(package_id),
    CONSTRAINT FK_Bookings_Destinations
        FOREIGN KEY (destination_id) REFERENCES Destinations(destination_id),
    CONSTRAINT FK_Bookings_Flights
        FOREIGN KEY (flight_id) REFERENCES Flights(flight_id)
);

-- =========================================
-- BOOKING_TRAVELLERS
-- =========================================
CREATE TABLE BookingTravellers (
    traveler_id INT IDENTITY PRIMARY KEY,
    booking_id INT NOT NULL,
    full_name NVARCHAR(100) NOT NULL,
    date_of_birth DATE NULL,
    passport_number NVARCHAR(50) NULL,
    nationality NVARCHAR(50) NULL,
    contact_phone NVARCHAR(30) NULL,
    CONSTRAINT FK_BookingTravellers_Bookings
        FOREIGN KEY (booking_id) REFERENCES Bookings(booking_id)
        ON DELETE CASCADE
);

-- =========================================
-- PAYMENTS
-- =========================================
CREATE TABLE Payments (
    payment_id INT IDENTITY PRIMARY KEY,
    booking_id INT NOT NULL,
    amount DECIMAL(10,2) NOT NULL,
    currency NVARCHAR(10) NOT NULL,
    gateway_type NVARCHAR(20) NOT NULL,
    transaction_status NVARCHAR(20) NOT NULL,
    transaction_reference NVARCHAR(100) NULL,
    paid_at DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_Payments_Bookings
        FOREIGN KEY (booking_id) REFERENCES Bookings(booking_id)
);

-- =========================================
-- POSTS
-- =========================================
CREATE TABLE Posts (
    post_id INT IDENTITY PRIMARY KEY,
    user_id INT NOT NULL,
    destination_id INT NULL,
    package_id INT NULL,
    title NVARCHAR(150) NOT NULL,
    content NVARCHAR(MAX) NULL,
    trip_type NVARCHAR(50) NULL,
    rating INT NULL,
    is_public BIT NOT NULL DEFAULT 1,
    created_at DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_Posts_Users
        FOREIGN KEY (user_id) REFERENCES Users(user_id),
    CONSTRAINT FK_Posts_Destinations
        FOREIGN KEY (destination_id) REFERENCES Destinations(destination_id),
    CONSTRAINT FK_Posts_Packages
        FOREIGN KEY (package_id) REFERENCES Packages(package_id)
);

-- =========================================
-- POST_MEDIA
-- =========================================
CREATE TABLE PostMedia (
    media_id INT IDENTITY PRIMARY KEY,
    post_id INT NOT NULL,
    media_url NVARCHAR(255) NOT NULL,
    media_type NVARCHAR(50) NOT NULL,
    sort_order INT NOT NULL DEFAULT 0,
    CONSTRAINT FK_PostMedia_Posts
        FOREIGN KEY (post_id) REFERENCES Posts(post_id)
        ON DELETE CASCADE
);

-- =========================================
-- FAVORITES
-- =========================================
CREATE TABLE Favorites (
    favorite_id INT IDENTITY PRIMARY KEY,
    user_id INT NOT NULL,
    destination_id INT NULL,
    package_id INT NULL,
    created_at DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_Favorites_Users
        FOREIGN KEY (user_id) REFERENCES Users(user_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_Favorites_Destinations
        FOREIGN KEY (destination_id) REFERENCES Destinations(destination_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_Favorites_Packages
        FOREIGN KEY (package_id) REFERENCES Packages(package_id)
        ON DELETE CASCADE
);

-- =========================================
-- REVIEWS (generic: package / destination)
-- =========================================
CREATE TABLE Reviews (
    review_id INT IDENTITY PRIMARY KEY,
    user_id INT NOT NULL,
    entity_type NVARCHAR(30) NOT NULL,  -- package/destination
    entity_id INT NOT NULL,
    rating INT NOT NULL,
    comment NVARCHAR(MAX) NULL,
    created_at DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_Reviews_Users
        FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- =========================================
-- NOTIFICATIONS
-- =========================================
CREATE TABLE Notifications (
    notification_id INT IDENTITY PRIMARY KEY,
    user_id INT NOT NULL,
    title NVARCHAR(150) NOT NULL,
    message NVARCHAR(MAX) NOT NULL,
    type NVARCHAR(50) NULL,
    is_read BIT NOT NULL DEFAULT 0,
    created_at DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_Notifications_Users
        FOREIGN KEY (user_id) REFERENCES Users(user_id)
        ON DELETE CASCADE
);

-- =========================================
-- USER_ACTIONS (for AI / tracking)
-- =========================================
CREATE TABLE UserActions (
    action_id INT IDENTITY PRIMARY KEY,
    user_id INT NOT NULL,
    destination_id INT NULL,
    package_id INT NULL,
    flight_id INT NULL,
    action_type NVARCHAR(50) NOT NULL,
    action_value DECIMAL(5,2) NULL,     -- weight/score
    action_timestamp DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_UserActions_Users
        FOREIGN KEY (user_id) REFERENCES Users(user_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_UserActions_Destinations
        FOREIGN KEY (destination_id) REFERENCES Destinations(destination_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_UserActions_Packages
        FOREIGN KEY (package_id) REFERENCES Packages(package_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_UserActions_Flights
        FOREIGN KEY (flight_id) REFERENCES Flights(flight_id)
);

-- =========================================
-- RECOMMENDATION_LOGS
-- =========================================
CREATE TABLE RecommendationLogs (
    rec_log_id INT IDENTITY PRIMARY KEY,
    user_id INT NOT NULL,
    destination_id INT NULL,
    package_id INT NULL,
    flight_id INT NULL,
    algorithm_version NVARCHAR(50) NOT NULL,
    score DECIMAL(5,3) NULL,
    shown_at DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    clicked BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_RecommendationLogs_Users
        FOREIGN KEY (user_id) REFERENCES Users(user_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_RecommendationLogs_Destinations
        FOREIGN KEY (destination_id) REFERENCES Destinations(destination_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_RecommendationLogs_Packages
        FOREIGN KEY (package_id) REFERENCES Packages(package_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_RecommendationLogs_Flights
        FOREIGN KEY (flight_id) REFERENCES Flights(flight_id)
);

-- =========================================
-- USER-INTERACTIONS
-- =========================================

CREATE TABLE UserInteractions (
    Id INT PRIMARY KEY IDENTITY,
    UserId INT NOT NULL,
    DestinationId INT NOT NULL,
    InteractionType NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE()
);
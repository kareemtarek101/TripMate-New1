using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using TripMate.Infrastructure.Persistence.Entities;

namespace TripMate.Infrastructure.Persistence;

public partial class TripMateDbContext : DbContext
{
    public TripMateDbContext()
    {
    }

    public TripMateDbContext(DbContextOptions<TripMateDbContext> options)
        : base(options)
    {
    }
    public DbSet<UserInteraction> UserInteractions { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Rating> Ratings { get; set; }


    public DbSet<Category> Categories { get; set; }

    public virtual DbSet<BookingTraveller> BookingTravellers { get; set; }

    public virtual DbSet<Destination> Destinations { get; set; }

    public virtual DbSet<DestinationMedium> DestinationMedia { get; set; }


    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<PackageFlight> PackageFlights { get; set; }

    public virtual DbSet<PackageMedium> PackageMedia { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostMedium> PostMedia { get; set; }

    public virtual DbSet<RecommendationLog> RecommendationLogs { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TripType> TripTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAction> UserActions { get; set; }

    public virtual DbSet<UserPreference> UserPreferences { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=KAREEM;Database=TripMate;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Bookings__5DE3A5B1F0325959");

            entity.Property(e => e.BookedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Destination).WithMany(p => p.Bookings).HasConstraintName("FK_Bookings_Destinations");

            entity.HasOne(d => d.Flight).WithMany(p => p.Bookings).HasConstraintName("FK_Bookings_Flights");

            entity.HasOne(d => d.Package).WithMany(p => p.Bookings).HasConstraintName("FK_Bookings_Packages");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookings_Users");
        });

        modelBuilder.Entity<BookingTraveller>(entity =>
        {
            entity.HasKey(e => e.TravelerId).HasName("PK__BookingT__53C729FA564ADB6C");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingTravellers).HasConstraintName("FK_BookingTravellers_Bookings");
        });

        modelBuilder.Entity<Destination>(entity =>
        {
            entity.HasKey(e => e.DestinationId).HasName("PK__Destinat__55015391DD0D892B");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.TripType).WithMany(p => p.Destinations).HasConstraintName("FK_Destinations_TripTypes");
        });

        modelBuilder.Entity<DestinationMedium>(entity =>
        {
            entity.HasKey(e => e.MediaId).HasName("PK__Destinat__D0A840F466876063");

            entity.HasOne(d => d.Destination).WithMany(p => p.DestinationMedia).HasConstraintName("FK_DestinationMedia_Destinations");
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.FavoriteId)
                .HasName("PK__Favorite__46ACF4CBA1E9B777");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Favorites)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Favorites_Users");
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.FlightId).HasName("PK__Flights__E3705765DA77E091");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__E059842F9E802E30");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications).HasConstraintName("FK_Notifications_Users");
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PK__Packages__63846AE84877CC47");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Destination).WithMany(p => p.Packages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Packages_Destinations");
        });

        modelBuilder.Entity<PackageFlight>(entity =>
        {
            entity.HasKey(e => e.PackageFlightId).HasName("PK__PackageF__51D92FC365C61F08");

            entity.HasOne(d => d.Flight).WithMany(p => p.PackageFlights)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageFlights_Flights");

            entity.HasOne(d => d.Package).WithMany(p => p.PackageFlights).HasConstraintName("FK_PackageFlights_Packages");
        });

        modelBuilder.Entity<PackageMedium>(entity =>
        {
            entity.HasKey(e => e.MediaId).HasName("PK__PackageM__D0A840F4725B70DD");

            entity.HasOne(d => d.Package).WithMany(p => p.PackageMedia).HasConstraintName("FK_PackageMedia_Packages");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__ED1FC9EA519FBE3F");

            entity.Property(e => e.PaidAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Bookings");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Posts__3ED7876658D7ED95");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsPublic).HasDefaultValue(true);

            entity.HasOne(d => d.Destination).WithMany(p => p.Posts).HasConstraintName("FK_Posts_Destinations");

            entity.HasOne(d => d.Package).WithMany(p => p.Posts).HasConstraintName("FK_Posts_Packages");

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Posts_Users");
        });

        modelBuilder.Entity<PostMedium>(entity =>
        {
            entity.HasKey(e => e.MediaId).HasName("PK__PostMedi__D0A840F4F9DE00FE");

            entity.HasOne(d => d.Post).WithMany(p => p.PostMedia).HasConstraintName("FK_PostMedia_Posts");
        });

        modelBuilder.Entity<RecommendationLog>(entity =>
        {
            entity.HasKey(e => e.RecLogId).HasName("PK__Recommen__056DBA0666E7D7E4");

            entity.Property(e => e.ShownAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Destination).WithMany(p => p.RecommendationLogs)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RecommendationLogs_Destinations");

            entity.HasOne(d => d.Flight).WithMany(p => p.RecommendationLogs).HasConstraintName("FK_RecommendationLogs_Flights");

            entity.HasOne(d => d.Package).WithMany(p => p.RecommendationLogs)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RecommendationLogs_Packages");

            entity.HasOne(d => d.User).WithMany(p => p.RecommendationLogs).HasConstraintName("FK_RecommendationLogs_Users");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__60883D905D1AAAAD");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reviews_Users");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__760965CCFF80CC4C");
        });

        modelBuilder.Entity<TripType>(entity =>
        {
            entity.HasKey(e => e.TripTypeId).HasName("PK__TripType__74B29FC1BF55D401");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370FB16FDC5D");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        modelBuilder.Entity<UserAction>(entity =>
        {
            entity.HasKey(e => e.ActionId).HasName("PK__UserActi__74EFC217D72B3519");

            entity.Property(e => e.ActionTimestamp).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Destination).WithMany(p => p.UserActions)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UserActions_Destinations");

            entity.HasOne(d => d.Flight).WithMany(p => p.UserActions).HasConstraintName("FK_UserActions_Flights");

            entity.HasOne(d => d.Package).WithMany(p => p.UserActions)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UserActions_Packages");

            entity.HasOne(d => d.User).WithMany(p => p.UserActions).HasConstraintName("FK_UserActions_Users");
        });

        modelBuilder.Entity<UserPreference>(entity =>
        {
            entity.HasKey(e => e.UserPrefId).HasName("PK__UserPref__BDC9BDBE1BE3CCF6");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.PreferredTripType).WithMany(p => p.UserPreferences).HasConstraintName("FK_UserPreferences_TripTypes");

            entity.HasOne(d => d.User).WithOne(p => p.UserPreference).HasConstraintName("FK_UserPreferences_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

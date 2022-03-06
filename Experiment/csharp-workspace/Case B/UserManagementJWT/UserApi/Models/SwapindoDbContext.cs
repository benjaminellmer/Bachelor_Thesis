using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace UserApi.Models {
    public partial class SwapindoDbContext : DbContext {
        public SwapindoDbContext() {
        }

        public SwapindoDbContext(DbContextOptions<SwapindoDbContext> options)
            : base(options) {
        }

        public virtual DbSet<BusinessOffice> BusinessOffices { get; set; }
        public virtual DbSet<BusinessOfficePicture> BusinessOfficePictures { get; set; }
        public virtual DbSet<BusinessUser> BusinessUsers { get; set; }
        public virtual DbSet<BusinessVerification> BusinessVerifications { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Credit> Credits { get; set; }
        public virtual DbSet<Follower> Followers { get; set; }
        public virtual DbSet<HighlightCredit> HighlightCredits { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<PrioCredit> PrioCredits { get; set; }
        public virtual DbSet<PrivateUser> PrivateUsers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserWatchList> UserWatchLists { get; set; }
        public virtual DbSet<Valuation> Valuations { get; set; }
        public virtual DbSet<Verification> Verifications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=swapindo.postgres.database.azure.com;Database=swapindodb;Username=swapindouser;Password=swapindodbuser; Ssl Mode=Require;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<BusinessOffice>(entity => {
                entity.ToTable("BusinessOffice", "UserManagement");

                entity.Property(e => e.BusinessOfficeId)
                    .HasColumnName("BusinessOfficeID")
                    .UseIdentityAlwaysColumn()
                    .HasIdentityOptions(null, null, null, null, true, null);

                entity.Property(e => e.BusinessUserId).HasColumnName("BusinessUserID");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.HasOne(d => d.BusinessUser)
                    .WithMany(p => p.BusinessOffices)
                    .HasForeignKey(d => d.BusinessUserId)
                    .HasConstraintName("BusinessUser_fkey");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.BusinessOffices)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Location_fkey");
            });

            modelBuilder.Entity<BusinessOfficePicture>(entity => {
                entity.ToTable("BusinessOfficePicture", "UserManagement");

                entity.Property(e => e.BusinessOfficePictureId)
                    .ValueGeneratedNever()
                    .HasColumnName("BusinessOfficePictureID");

                entity.Property(e => e.BusinessOfficeId).HasColumnName("BusinessOfficeID");

                entity.HasOne(d => d.BusinessOffice)
                    .WithMany(p => p.BusinessOfficePictures)
                    .HasForeignKey(d => d.BusinessOfficeId)
                    .HasConstraintName("BusinessOffice_fkey");
            });

            modelBuilder.Entity<BusinessUser>(entity => {
                entity.ToTable("BusinessUser", "UserManagement");

                entity.Property(e => e.BusinessUserId)
                    .ValueGeneratedNever()
                    .HasColumnName("BusinessUserID");

                entity.Property(e => e.BusinessPackageId).HasColumnName("BusinessPackageID");

                entity.Property(e => e.BusinessVerificationId).HasColumnName("BusinessVerificationID");

                entity.HasOne(d => d.BusinessVerification)
                    .WithMany(p => p.BusinessUsers)
                    .HasForeignKey(d => d.BusinessVerificationId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("BusinessVerification_fkey");
            });

            modelBuilder.Entity<BusinessVerification>(entity => {
                entity.ToTable("BusinessVerification", "UserManagement");

                entity.Property(e => e.BusinessVerificationId)
                    .ValueGeneratedNever()
                    .HasColumnName("BusinessVerificationID");

                entity.Property(e => e.DocumentMediaId).HasColumnName("DocumentMediaID");

                entity.Property(e => e.RequestDate).HasDefaultValueSql("(now())::timestamp without time zone");
            });

            modelBuilder.Entity<Contact>(entity => {
                entity.ToTable("Contact", "UserManagement");

                entity.Property(e => e.ContactId)
                    .ValueGeneratedNever()
                    .HasColumnName("ContactID");

                entity.Property(e => e.Gender).HasMaxLength(1);

                entity.Property(e => e.PrimaryLocationId).HasColumnName("PrimaryLocationID");

                entity.HasOne(d => d.PrimaryLocation)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.PrimaryLocationId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("PrimaryLocation_fkey");
            });

            modelBuilder.Entity<Credit>(entity => {
                entity.ToTable("Credit", "UserManagement");

                entity.Property(e => e.CreditId)
                    .ValueGeneratedNever()
                    .HasColumnName("CreditID");
            });

            modelBuilder.Entity<Follower>(entity => {
                entity.HasKey(e => new { e.FromUserId, e.ToUserId })
                    .HasName("Follower_pkey");

                entity.ToTable("Follower", "UserManagement");

                entity.Property(e => e.FromUserId).HasColumnName("FromUserID");

                entity.Property(e => e.ToUserId).HasColumnName("ToUserID");

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.FollowerFromUsers)
                    .HasForeignKey(d => d.FromUserId)
                    .HasConstraintName("FromUser_pkey");

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.FollowerToUsers)
                    .HasForeignKey(d => d.ToUserId)
                    .HasConstraintName("ToUser_pkey");
            });

            modelBuilder.Entity<HighlightCredit>(entity => {
                entity.ToTable("HighlightCredit", "UserManagement");

                entity.Property(e => e.HighlightCreditId)
                    .HasColumnName("HighlightCreditID")
                    .UseIdentityAlwaysColumn()
                    .HasIdentityOptions(null, null, null, null, true, null);

                entity.Property(e => e.CreditId).HasColumnName("CreditID");

                entity.HasOne(d => d.Credit)
                    .WithMany(p => p.HighlightCredits)
                    .HasForeignKey(d => d.CreditId)
                    .HasConstraintName("Credit_fkey");
            });

            modelBuilder.Entity<Location>(entity => {
                entity.ToTable("Location", "UserManagement");

                entity.Property(e => e.LocationId)
                    .HasColumnName("LocationID")
                    .UseIdentityAlwaysColumn()
                    .HasIdentityOptions(null, null, null, null, true, null);

                entity.Property(e => e.City).IsRequired();

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.Country).IsRequired();

                entity.Property(e => e.Street).IsRequired();

                entity.Property(e => e.StreetNumber).IsRequired();

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Contact_fkey");
            });

            modelBuilder.Entity<PrioCredit>(entity => {
                entity.ToTable("PrioCredit", "UserManagement");

                entity.Property(e => e.PrioCreditId)
                    .HasColumnName("PrioCreditID")
                    .UseIdentityAlwaysColumn()
                    .HasIdentityOptions(null, null, null, null, true, null);

                entity.Property(e => e.CreditId).HasColumnName("CreditID");

                entity.HasOne(d => d.Credit)
                    .WithMany(p => p.PrioCredits)
                    .HasForeignKey(d => d.CreditId)
                    .HasConstraintName("Credit_pkey");
            });

            modelBuilder.Entity<PrivateUser>(entity => {
                entity.ToTable("PrivateUser", "UserManagement");

                entity.Property(e => e.PrivateUserId)
                    .ValueGeneratedNever()
                    .HasColumnName("PrivateUserID");

                entity.Property(e => e.PremiumId).HasColumnName("PremiumID");
            });

            modelBuilder.Entity<User>(entity => {
                entity.ToTable("User", "UserManagement");

                entity.HasIndex(e => e.Email, "UniqueEmail")
                    .IsUnique();

                entity.HasIndex(e => e.Uid, "UniqueUID")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .UseIdentityAlwaysColumn()
                    .HasIdentityOptions(null, null, null, null, true, null);

                entity.Property(e => e.BusinessUserId).HasColumnName("BusinessUserID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CreditId).HasColumnName("CreditID");

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("true");

                entity.Property(e => e.PrivateUserId).HasColumnName("PrivateUserID");

                entity.Property(e => e.ProfilePictureMediaId).HasColumnName("ProfilePictureMediaID");

                entity.Property(e => e.Uid)
                    .IsRequired()
                    .HasColumnName("UID");

                entity.Property(e => e.VerificationId).HasColumnName("VerificationID");

                entity.HasOne(d => d.BusinessUser)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.BusinessUserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("BusinessUser_fkey");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Contact_fkey");

                entity.HasOne(d => d.Credit)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.CreditId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Credit_fkey");

                entity.HasOne(d => d.PrivateUser)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.PrivateUserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("PrivateUser_fkey");

                entity.HasOne(d => d.Verification)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.VerificationId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Verification_fkey");
            });

            modelBuilder.Entity<UserWatchList>(entity => {
                entity.HasKey(e => new { e.UserId, e.AdId })
                    .HasName("UserWatchList_pkey");

                entity.ToTable("UserWatchList", "UserManagement");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.DateOfCreation)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(now())::date");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserWatchLists)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("User_fkey");
            });

            modelBuilder.Entity<Valuation>(entity => {
                entity.ToTable("Valuation", "UserManagement");

                entity.Property(e => e.ValuationId)
                    .HasColumnName("ValuationID")
                    .UseIdentityAlwaysColumn()
                    .HasIdentityOptions(null, null, null, null, true, null);

                entity.Property(e => e.FromUserId).HasColumnName("FromUserID");

                entity.Property(e => e.ToUserId).HasColumnName("ToUserID");

                entity.Property(e => e.ValuationValue).HasPrecision(3, 2);

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.ValuationFromUsers)
                    .HasForeignKey(d => d.FromUserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FromUser_fkey");

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.ValuationToUsers)
                    .HasForeignKey(d => d.ToUserId)
                    .HasConstraintName("ToUser_fkey");
            });

            modelBuilder.Entity<Verification>(entity => {
                entity.ToTable("Verification", "UserManagement");

                entity.Property(e => e.VerificationId)
                    .ValueGeneratedNever()
                    .HasColumnName("VerificationID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

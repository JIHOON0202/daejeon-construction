using DaejeonConstruction.Web.Models;
using DaejeonConstruction.Web.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DaejeonConstruction.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AdminUser> AdminUsers => Set<AdminUser>();
        public DbSet<MainBanner> MainBanners => Set<MainBanner>();
        public DbSet<WorkCase> WorkCases => Set<WorkCase>();
        public DbSet<WorkImage> WorkImages => Set<WorkImage>();
        public DbSet<EstimateRequest> EstimateRequests => Set<EstimateRequest>();
        public DbSet<EstimateFile> EstimateFiles => Set<EstimateFile>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AdminUser>(e =>
            {
                e.ToTable("ADMIN_USER");
                e.HasIndex(x => x.Username).IsUnique();
            });

            modelBuilder.Entity<MainBanner>(e =>
            {
                e.ToTable("MAIN_BANNER");
            });

            modelBuilder.Entity<WorkCase>(e =>
            {
                e.ToTable("WORK_CASE");
                e.Property(x => x.Category)
                    .HasConversion<string>()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<WorkImage>(e =>
            {
                e.ToTable("WORK_IMAGE");
                e.Property(x => x.ImageType)
                    .HasConversion<string>()
                    .HasMaxLength(20);
                e.HasOne(x => x.WorkCase)
                    .WithMany(x => x.Images)
                    .HasForeignKey(x => x.WorkCaseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<EstimateRequest>(e =>
            {
                e.ToTable("ESTIMATE_REQUEST");
                e.Property(x => x.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<EstimateFile>(e =>
            {
                e.ToTable("ESTIMATE_FILE");
                e.HasOne(x => x.EstimateRequest)
                    .WithMany(x => x.Files)
                    .HasForeignKey(x => x.EstimateRequestId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}

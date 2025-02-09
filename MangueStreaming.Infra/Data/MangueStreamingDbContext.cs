using Microsoft.EntityFrameworkCore;
using MangueStreaming.Domain.Models;

namespace MangueStreaming.Infra.Data
{
    public class MangueStreamingDbContext : DbContext
    {
        public MangueStreamingDbContext(DbContextOptions<MangueStreamingDbContext> options)
            : base(options)
        {
        }
        public DbSet<Video> Videos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Video>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.Property(v => v.Title).IsRequired().HasMaxLength(200);
                entity.Property(v => v.Description).HasMaxLength(1000);
                entity.Property(v => v.Url).IsRequired();
                entity.Property(v => v.UploadedAt).IsRequired();
            });
        }
    }
}

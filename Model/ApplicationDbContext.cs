using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.Model;
using MyApi1.Model;

namespace MyApi.Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=.;Database=MolkeAriaeiDb;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Member>()
                .HasKey(a => a.Id);
            modelBuilder
                .Entity<Setting>()
                .HasKey(a => a.Id);
            modelBuilder
                .Entity<Project>()
                .HasKey(a => a.Id);
            modelBuilder
                .Entity<Customer>()
                .HasKey(a => a.Id);
            modelBuilder
                .Entity<AppUser>()
                .HasKey(a => a.UserId);
            modelBuilder
                .Entity<City>()
                .HasKey(a => a.Id);
            modelBuilder
                .Entity<Estate>()
                .HasKey(a => a.Id);
            modelBuilder
                .Entity<EstateGallery>()
                .HasKey(a => a.Id);
            modelBuilder
                .Entity<Slider>()
                .HasKey(a => a.Id);
            modelBuilder
                .Entity<Town>()
                .HasKey(a => a.Id);
            modelBuilder
                .Entity<TownGallery>()
                .HasKey(a => a.Id);
            modelBuilder
                .Entity<Zone>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<Estate>()
              .HasOne<City>(s => s.City)
              .WithMany(a => a.Estates)
              .HasForeignKey(s => s.CityId);
            modelBuilder.Entity<Estate>()
              .HasOne<City>(s => s.City)
              .WithMany(a => a.Estates)
              .HasForeignKey(s => s.ProvinceId);
            modelBuilder.Entity<EstateGallery>()
              .HasOne<Estate>(s => s.Estate)
              .WithMany(a => a.EstateGalleries)
              .HasForeignKey(s => s.EstateId);
            modelBuilder.Entity<TownGallery>()
              .HasOne<Town>(s => s.Town)
              .WithMany(a => a.TownGalleries)
              .HasForeignKey(s => s.TownId);
            modelBuilder.Entity<Town>()
              .HasOne<City>(s => s.City)
              .WithMany(a => a.Towns)
              .HasForeignKey(s => s.CityId);
            modelBuilder.Entity<Town>()
              .HasOne<City>(s => s.City)
              .WithMany(a => a.Towns)
              .HasForeignKey(s => s.ProvinceId);
            modelBuilder.Entity<Zone>()
              .HasOne<City>(s => s.City)
              .WithMany(a => a.Zones)
              .HasForeignKey(s => s.CityId);
        }

        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<Estate> Estates { get; set; }
        public virtual DbSet<EstateGallery> EstateGalleries { get; set; }
        public virtual DbSet<Slider> Sliders { get; set; }
        public virtual DbSet<Town> Towns { get; set; }
        public virtual DbSet<TownGallery> TownGalleries { get; set; }
        public virtual DbSet<Zone> Zones { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Setting> Setting { get; set; }
    }
}

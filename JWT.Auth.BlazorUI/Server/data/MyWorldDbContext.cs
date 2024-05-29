using JWT.Auth.BlazorUI.Client.Pages;
using JWT.Auth.BlazorUI.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JWT.Auth.BlazorUI.Server.data
{
    public class MyWorldDbContext : DbContext
    {
        public MyWorldDbContext(DbContextOptions<MyWorldDbContext> options) : base(options)
        {

        }
        public DbSet<User> User { get; set; }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            var timeOnlyConverter = new ValueConverter<TimeOnly, string>(
                v => v.ToString("HH:mm"),
                v => TimeOnly.Parse(v)
            );

            modelBuilder.Entity<Appointment>()
                .Property(a => a.Time)
                .HasConversion(timeOnlyConverter);

            base.OnModelCreating(modelBuilder);
        }

    }
}

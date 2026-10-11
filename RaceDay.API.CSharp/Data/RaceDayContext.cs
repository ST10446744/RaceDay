using Microsoft.EntityFrameworkCore;
using RaceDay.API.CSharp.Models;

namespace RaceDay.API.CSharp.Data
{
    public class RaceDayContext : DbContext
    {
        public RaceDayContext(DbContextOptions<RaceDayContext> options)
        : base(options)
        {
        }


    public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Enrolment> Enrolments { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<EventDocument> EventDocuments { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<WeatherInformation> WeatherInformation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organiser)
                .WithMany()
                .HasForeignKey(e => e.OrganiserID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Enrolment>()
                .HasOne(e => e.Participant)
                .WithMany()
                .HasForeignKey(e => e.ParticipantID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Enrolment>()
                .HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EventDocument>()
                .HasOne(d => d.Event)
                .WithMany()
                .HasForeignKey(d => d.EventID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Enrolment)
                .WithMany()
                .HasForeignKey(p => p.EnrolmentID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<WeatherInformation>()
                .HasOne(w => w.Event)
                .WithMany()
                .HasForeignKey(w => w.EventID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }


}

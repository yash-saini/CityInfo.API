using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    public class CityInfoContext :DbContext
    {
        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>() 
                .HasData(
                    new City("NYC")
                    {
                        Id = 1,
                        Name = "New York City",
                        Description = "The one with that big park."
                    },
                    new City("Antwerp")
                    {
                        Id = 2,
                        Name = "Antwerp",
                        Description = "The one with the cathedral that was never really finished."
                    },
                    new City("Paris")
                    {
                        Id = 3,
                        Name = "Paris",
                        Description = "The one with that big tower."
                    }
                );
            modelBuilder.Entity<PointOfInterest>().HasData(
                new PointOfInterest("Central Park")
                {
                    Id = 1,
                    CityId = 1
                },
                new PointOfInterest("Empire State Building")
                {
                    Id = 2,
                    CityId = 1
                },
                new PointOfInterest("Cathedral of Our Lady")
                {
                    Id = 3,
                    CityId = 2
                });
            base.OnModelCreating(modelBuilder);
        }
    }
}

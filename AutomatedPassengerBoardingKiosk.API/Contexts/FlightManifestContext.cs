using AutomatedPassengerBoardingKiosk.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutomatedPassengerBoardingKiosk.API.Contexts
{
    public class FlightManifestContext : DbContext
    {
        public FlightManifestContext(DbContextOptions<FlightManifestContext> options) : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<BoardingPass> BoardingPasses { get; set; }
    }
}

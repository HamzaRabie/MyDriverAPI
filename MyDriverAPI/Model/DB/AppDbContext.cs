using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyDriver.Model.AuthManagment;
using MyDriver.Model.DriversData;
using MyDriver.Model.PassengersData;
using MyDriverAPI.Model.NotificationsAndOffers;
using MyDriverAPI.Model.Trip;


namespace MyDriver.Model.DB
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Driver> drivers { get; set; }
        public DbSet<Passenger> passengers { get; set; }
        public DbSet<Trip> trips { get; set; }
        public DbSet<NotificationModel> notifications { get; set; }

    }
}

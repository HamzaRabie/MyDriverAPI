
using Microsoft.EntityFrameworkCore;
using MyDriver.Model.DB;
using MyDriver.Model.DriversData;
using MyDriver.Model.PassengersData;
using MyDriverAPI.Model.Trip;
using MyDriverAPI.repository.Interfaces;

namespace MyDriverAPI.Services.TripServices
{
    public class TripService : ITripService
    {
        private readonly IDriverRepo driverRepo;
        private readonly AppDbContext context;
        private List<Driver> driversList;

        public TripService( IDriverRepo driverRepo , AppDbContext context )
        {
            this.driverRepo = driverRepo;
            this.context = context;
            driversList = context.drivers.ToList();
        }



        public async Task AddAsync(DriverRegister newDriver)
        {
            var driverDb = await context.drivers.SingleOrDefaultAsync(d => d.UserName.Equals(newDriver.UserName));
            driversList.Add(driverDb);
            var size = driversList.Count;
            Console.WriteLine(size);
        }

        public async Task DeleteAsync(DriverRegister driver)
        {
            var driverDb = await context.drivers.SingleOrDefaultAsync(d => d.UserName == driver.UserName);
            driversList.Remove(driverDb);
        }
        
        public async Task NotifyAsync( string Location , string Destination , Passenger passenger)
        {
            foreach(var driver in driversList)
            {
                foreach( var area in driver.FavAreas)
                {
                    if( area.ToLower().Equals(Location.ToLower()) ) {
                         
                     await driverRepo.UpdateNotificationAsync(driver, Location, Destination , passenger);
                    }
                }
            }
            context.SaveChanges();
        }
        
        public async Task requestTrip(string Location, string Destination , string Id)
        {
            var passenger = await context.passengers.SingleAsync(p => p.AppUserId == Id);
            await NotifyAsync(Location, Destination , passenger);
        }

    }
}

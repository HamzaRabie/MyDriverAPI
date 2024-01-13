using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyDriver.Model.DB;
using MyDriver.Model.DriversData;
using MyDriver.Model.PassengersData;
using MyDriver.repository.Classess;
using MyDriverAPI.Model.DriversData;
using MyDriverAPI.Model.NotificationsAndOffers;
using MyDriverAPI.Model.PassengersData;
using MyDriverAPI.Model.Trip;
using MyDriverAPI.repository.Interfaces;

namespace MyDriverAPI.repository.Classess
{
    public class DriverRepo : BaseRepo<Driver>, IDriverRepo
    {
        private readonly AppDbContext context;

        public DriverRepo( AppDbContext context ) : base( context )
        {
            this.context = context;
        }
        public async Task<DriverUpdateModel> UpdateDriver(DriverUpdateModel newDriver, string driverId)
        {
            var driverDb = await context.drivers.SingleAsync(d=>d.AppUserId.Equals(driverId));

            if (driverDb == null) 
                return null;

            driverDb.Email = newDriver.Email;
            driverDb.FirstName = newDriver.FirstName;
            driverDb.LastName = newDriver.LastName;
            driverDb.PhoneNumber = newDriver.PhoneNumber;
            driverDb.UserName = newDriver.UserName;
            driverDb.Password = newDriver.Password;
            driverDb.LicenseNumber = newDriver.LicenseNumber;
          //  driverDb.CarPhoto = newDriver.CarPhoto;
            driverDb.FavAreas = newDriver.FavAreas;

            return newDriver;
        }

        public async Task<List<string>> AddNewFavArea(string area , string driverId)
        {
            var cuurDriver = await context.drivers.SingleAsync(d => d.AppUserId == driverId);
            cuurDriver.FavAreas.Add(area.ToLower());

            return cuurDriver.FavAreas;
        }

        public async Task<bool> DeleteArea(string area, string driverId)
        {
            var cuurDriver = await context.drivers.SingleAsync(d => d.AppUserId == driverId);
            var res = cuurDriver.FavAreas.Remove(area);
            return res;

        }

        
        public async Task UpdateNotificationAsync(Driver driver, string Location, string Destination , Passenger passenger)
        {
            
            if (driver.Noitfications == null)
                driver.Noitfications = new List<NotificationModel>();
 
            var model = new NotificationModel()
            {
                Notification = $"there is trip from {Location} to {Destination} You want to offer price ? ",
                PassengerName = passenger.UserName,
                DriverName = driver.UserName,
                Location= Location,
                Destination = Destination,
            };

            driver.Noitfications.Add(model);
            await context.SaveChangesAsync();
        }
        
        public async Task<List<NotificationModel>> ShowNotificationAsync(string driverId)
        {
            var driver = await context.drivers.Include(d=>d.Noitfications).SingleAsync(d=>d.AppUserId == driverId);
            if (driver.Noitfications.Count != 0)
                return driver.Noitfications;

            return null;
        }
        
        public async Task<string> OfferPrice( OfferPriceModel model,string driverId)
        {
            var driver = await context.drivers.Include(d=>d.Noitfications).SingleAsync( d=>d.AppUserId == driverId );
            if (model.NotificationNumber > driver.Noitfications.Count || model.Price == 0)
                return "Please enter valid notification number ";

            driver.Noitfications[model.NotificationNumber - 1].Price = model.Price;
            context.SaveChanges();
            return " Offer sent successfully , please check status for updates ";
        }

        public async Task<TripModel> CheckStatus(string driverName)
        {
            var trip = await context.trips.FirstOrDefaultAsync( t=>t.DriverName == driverName);
            if (trip == null)
                return null;

            var passenger = await context.passengers.SingleAsync(p => p.UserName == trip.PassengerName);
            return new TripModel
            {
                Location = trip.Location,
                Destination = trip.Destination,
                 PassengerEmail= passenger.Email,
                 PassengerFirstName= passenger.FirstName,
                 PassengerLastName= passenger.LastName,
                 PassengerPhoneNumber= passenger.PhoneNumber,
                 PassengerUserName = passenger.UserName,
            };
        }

        public async Task<IEnumerable<TripDriverModel>> ShowTrips(string driverId)
        {
            var driver = await context.drivers.Include(d=>d.Trips).SingleOrDefaultAsync( d=>d.AppUserId == driverId );
            if (driver == null)
                return null;

            List<TripDriverModel> trips = new List<TripDriverModel>();
            foreach(var trip in driver.Trips)
            {
                trips.Add( new TripDriverModel {
                    Location = trip.Location,
                    Destination = trip.Destination,
                    Price = trip.Price,
                    PassengerName = trip.PassengerName,
                } );
            }

            return trips;

        }


    }
}

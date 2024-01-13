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
    public class PassengerRepo : BaseRepo<Passenger> , IPassengerRepo
    {
        private readonly AppDbContext context;

        public PassengerRepo(AppDbContext context) : base(context) 
        {
            this.context = context;
        }

        public async Task<PassengerUpdateModel> UpdatePassenger(PassengerUpdateModel newPassenger, string passengerId)
        {
            var passengerDb = await context.passengers.SingleAsync(d => d.AppUserId.Equals(passengerId));

            if (passengerDb == null)
                return null;

            passengerDb.Email = newPassenger.Email;
            passengerDb.FirstName = newPassenger.FirstName;
            passengerDb.LastName = newPassenger.LastName;
            passengerDb.PhoneNumber = newPassenger.PhoneNumber;
            passengerDb.UserName = newPassenger.UserName;
            passengerDb.Password = newPassenger.Password;
            return newPassenger;
        }

        
        public async Task<List<ShowOffersModel>> ShowOffers(string name)
        {
            var offersList = await context.notifications.Where(n => n.PassengerName == name).ToListAsync();
            List<ShowOffersModel> offers = new List<ShowOffersModel>();
            foreach( var offer in offersList)
            {
                if( offer.Price != null)
                {
                    var driverDb = await context.drivers.SingleAsync(d => d.UserName == offer.DriverName);
                    offers.Add(new ShowOffersModel
                    {   
                        driver = new DriverInfoModel
                        {
                            Email = driverDb.Email,
                            FirstName = driverDb.FirstName,
                            LastName = driverDb.LastName,
                            PhoneNumber = driverDb.PhoneNumber,
                            UserName = driverDb.UserName,
                            Rating = driverDb.Rating??0
                            //to do car photo
                        },
                        price = (double)offer.Price,
                        Location = offer.Location,
                        Destination = offer.Destination
                    });
                }
            }
            return offers;
        }
        
        public async Task<DriverInfoModel> ChooseOffer(int offerNumber , string name)
        {
            var offersList = await context.notifications.Where(n => n.PassengerName == name && n.Price !=null ).ToListAsync();
            if (offerNumber > offersList.Count || offerNumber==0)
                    return null;
            var selectedOffer = offersList[offerNumber - 1];

            Trip trip = new Trip
            {
                Location = selectedOffer.Location,
                Destination = selectedOffer.Destination,
                //IsCompleted = false,
                Price = (double)selectedOffer.Price,
                DriverName = selectedOffer.DriverName,
                PassengerName = selectedOffer.PassengerName

            };
           
            var passenger = await context.passengers.Include(p=>p.Trips).SingleAsync(p => p.UserName == name);

            if (passenger.Trips == null)
                passenger.Trips = new List<Trip>();

            await context.trips.AddAsync(trip);
            passenger.Trips.Add(trip);

            var driver = await context.drivers.SingleAsync(d => d.UserName == selectedOffer.DriverName);

            if (driver.Trips == null)
                driver.Trips = new List<Trip>();

            driver.Trips.Add(trip);

            var notificatioList = await context.notifications.Where(n => n.PassengerName == name).ToListAsync();
            context.notifications.RemoveRange(notificatioList);

            context.SaveChanges();

            return new DriverInfoModel
            {
                FirstName = driver.FirstName,
                Email = driver.Email,
                LastName = driver.LastName,
                UserName = driver.UserName,
                PhoneNumber = driver.PhoneNumber,
                //to do car photo
            };

            //to do 
            //return driver data ( new model has info to communicate + know more about) + the same when show offers

        }

        public async Task<bool> RateDriver(string name , int rating)
        {
            if (rating < 0 || rating > 5)
                return false;

            var trip = await context.trips.OrderByDescending(t=>t.Id).FirstOrDefaultAsync( t=>t.PassengerName == name);
            var driver = await context.drivers.Include(p=>p.Trips).SingleAsync(d=>d.UserName == trip.DriverName);

            if (driver.Rating == null)
                driver.Rating = 0;

            driver.Rating = (driver.Rating + rating) / driver.Trips.Count();
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<TripPassengerModel>> ShowTrips(string PassengerId)
        {
            var passenger = await context.passengers.Include(d => d.Trips).SingleOrDefaultAsync(d => d.AppUserId == PassengerId);
            if (passenger == null)
                return null;

            List<TripPassengerModel> trips = new List<TripPassengerModel>();
            foreach (var trip in passenger.Trips)
            {
                trips.Add(new TripPassengerModel
                {
                    Location = trip.Location,
                    Destination = trip.Destination,
                    Price = trip.Price,
                     DriverName = trip.DriverName,
                });
            }

            return trips;
        }


    }
}

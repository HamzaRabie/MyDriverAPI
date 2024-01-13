using MyDriver.Model.DriversData;
using MyDriver.Model.PassengersData;
using MyDriver.repository.Interfaces;
using MyDriverAPI.Model.DriversData;
using MyDriverAPI.Model.NotificationsAndOffers;
using MyDriverAPI.Model.PassengersData;
using MyDriverAPI.Model.Trip;

namespace MyDriverAPI.repository.Interfaces
{
    public interface IPassengerRepo : IBaseRepo<Passenger>
    {
        Task<PassengerUpdateModel> UpdatePassenger(PassengerUpdateModel newDriPassenger, string passengerId);
        Task<List<ShowOffersModel>> ShowOffers( string name );
        Task<DriverInfoModel> ChooseOffer(int offerNumber, string Id);
        Task<List<TripPassengerModel>> ShowTrips(string PassengerId);
        Task<bool> RateDriver(string name , int rating);

    }
}

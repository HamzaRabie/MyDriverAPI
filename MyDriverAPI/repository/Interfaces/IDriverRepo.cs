using MyDriver.Model.DriversData;
using MyDriver.Model.PassengersData;
using MyDriver.repository.Interfaces;
using MyDriverAPI.Model.DriversData;
using MyDriverAPI.Model.NotificationsAndOffers;
using MyDriverAPI.Model.PassengersData;
using MyDriverAPI.Model.Trip;

namespace MyDriverAPI.repository.Interfaces
{
    public interface IDriverRepo : IBaseRepo<Driver>
    {
        Task<DriverUpdateModel> UpdateDriver(DriverUpdateModel newDriver, string driverId);
        Task<List<string>> AddNewFavArea(string area, string driverId);
        Task<bool> DeleteArea(string area, string driverId);
        Task UpdateNotificationAsync(Driver driver, string Location, string Destination, Passenger passenger);
        Task<List<NotificationModel>> ShowNotificationAsync(string driverId);
        Task<string> OfferPrice(OfferPriceModel model, string driverId);
        Task<TripModel> CheckStatus(string driverName);
        Task<IEnumerable<TripDriverModel>> ShowTrips(string driverId);
    }
}

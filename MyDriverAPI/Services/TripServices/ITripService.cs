using MyDriver.Model.DriversData;
using MyDriver.Model.PassengersData;

namespace MyDriverAPI.Services.TripServices
{
    public interface ITripService
    {
        Task AddAsync(DriverRegister newDriver);
        Task DeleteAsync(DriverRegister driver);
        Task NotifyAsync(string Location, string Destination, Passenger passenger);
        Task requestTrip(string Location, string Destination, string Id);
    }
}

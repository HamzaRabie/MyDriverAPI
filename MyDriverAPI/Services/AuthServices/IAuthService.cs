using MyDriver.Controllers.DTOS;
using MyDriver.Model.AuthManagment;
using MyDriver.Model.DriversData;
using MyDriver.Model.PassengersData;
using MyDriverAPI.Model.DriversData;
using MyDriverAPI.Model.PassengersData;

namespace MyDriverAPI.Services.AuthServices
{
    public interface IAuthService
    {
        Task<AuthModel> RegiserPassenger(PassengerRegister newPassenger);
        Task<AuthModel> RegiserDriver(DriverRegister newDriver);
        Task<AuthModel> Login(LoginDTO user);
        Task<DriverUpdateModel> UpdateDriver(DriverUpdateModel newDriver, string driverId);
        Task<PassengerUpdateModel> UpdatePassenger(PassengerUpdateModel newPassenger, string passengerId);
        Task<AuthModel> RefreshToken(string token);
        Task<bool> RevokeRefreshToken(string token);
        Task<bool> DeleteUser(string name);
    }
}

using MyDriver.Model.DriversData;
using MyDriver.Model.PassengersData;
using MyDriverAPI.repository.Interfaces;

namespace MyDriver.repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDriverRepo drivers { get; }
        IPassengerRepo passengers { get; }

        int complete();
    }
}

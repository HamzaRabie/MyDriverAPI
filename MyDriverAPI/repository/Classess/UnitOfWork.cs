using MyDriver.Model.DB;
using MyDriver.Model.DriversData;
using MyDriver.Model.PassengersData;
using MyDriver.repository.Interfaces;
using MyDriverAPI.repository.Classess;
using MyDriverAPI.repository.Interfaces;

namespace MyDriver.repository.Classess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        public IDriverRepo drivers { get; private set; }
        public IPassengerRepo passengers { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
            this.drivers = new DriverRepo(context);
            this.passengers = new PassengerRepo(context);
        }
       
        public int complete()
        {
            return context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}

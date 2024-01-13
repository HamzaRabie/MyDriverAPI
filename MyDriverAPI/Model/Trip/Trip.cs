using MyDriver.Model.DriversData;
using MyDriver.Model.PassengersData;

namespace MyDriverAPI.Model.Trip
{
    public class Trip
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string Destination { get; set; }
        public  string DriverName { get; set; }
        public  string PassengerName { get; set; }
        public  double Price { get; set; }
      // public  bool IsCompleted { get; set; }
        public  List<Driver> Drivers { get; set; }
        public List<Passenger> Passengers { get; set; }

    }
}

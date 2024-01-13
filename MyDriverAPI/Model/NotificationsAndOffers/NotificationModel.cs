using MyDriver.Model.DriversData;
using MyDriver.Model.PassengersData;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyDriverAPI.Model.NotificationsAndOffers
{
    public class NotificationModel
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Location { get; set; }
        public  string Destination { get; set; }
        public string PassengerName { get; set; }
        [JsonIgnore]
        public string DriverName { get; set; }
        public string Notification { get; set; }
        [JsonIgnore]
        public double? Price { get; set; }
        [JsonIgnore]
        public  List<Driver> Drivers { get; set; }
        //public List<Passenger> Passengers { get; set; }

    }
}

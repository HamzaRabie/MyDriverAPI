using MyDriver.Model.DriversData;
using MyDriverAPI.Model.DriversData;

namespace MyDriverAPI.Model.NotificationsAndOffers
{
    public class ShowOffersModel
    {
        //to add driverModel istead of driver
        public  double price { get; set; }
        public  string Location { get; set; }
        public  string Destination { get; set; }
        public DriverInfoModel driver { get; set; }

    }
}

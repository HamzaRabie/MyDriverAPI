namespace MyDriverAPI.Model.Trip
{
    public class TripModel
    {
        public  string Location { get; set; }
        public  string Destination { get; set; }
        public string PassengerUserName { get; set; }
        public string PassengerFirstName { get; set; }
        public string PassengerLastName { get; set; }
        public string PassengerEmail { get; set; }
        public string PassengerPhoneNumber { get; set; }
        public double Price { get; set; }
    }
}

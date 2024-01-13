using MyDriverAPI.Model.NotificationsAndOffers;
using MyDriverAPI.Model.Trip;

namespace MyDriver.Model.PassengersData
{
    public class Passenger
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        //public List<NotificationModel>? Noitfications { get; set; }
        public List<Trip> Trips { get; set; }
        public string? AppUserId { get; set; }

        public static explicit operator Passenger(ValueTask<Passenger?> v)
        {
            throw new NotImplementedException();
        }
    }
}

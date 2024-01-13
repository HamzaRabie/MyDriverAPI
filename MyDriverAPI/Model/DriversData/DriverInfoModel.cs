namespace MyDriverAPI.Model.DriversData
{
    public class DriverInfoModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public double Rating { get; set; }
        public byte[]? CarPhoto { get; set; }
    }
}

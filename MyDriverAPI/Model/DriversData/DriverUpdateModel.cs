namespace MyDriverAPI.Model.DriversData
{
    public class DriverUpdateModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
       // public byte[]? CarPhoto { get; set; }
        public string LicenseNumber { get; set; }
        public List<string>? FavAreas { get; set; }
        public  string? Message { get; set; }

    }
}

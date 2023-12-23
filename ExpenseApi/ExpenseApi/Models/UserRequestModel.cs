namespace ExpenseApi.Models
{
    public class UserRequestModel
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? BirthDate { get; set; }
        public AddressModel Address { get; set; }
    }

    public class AddressModel
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
    }
}

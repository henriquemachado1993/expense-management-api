using BeireMKit.Data.Interfaces.Entity;

namespace ExpenseApi.Domain.Entities
{
    public class User : IBaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Password { get; set; }
        public string JwtToken { get; set; }
        public Address Address { get; set; }

        public void ClearPassword(bool clearToken = true) {
            Password = string.Empty;
            if(clearToken)
                JwtToken = string.Empty; 
        }
    }

    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
    }
}

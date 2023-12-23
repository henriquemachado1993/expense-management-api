namespace ExpenseApi.Models
{
    public class UpdatePasswordRequestModel
    {
        public Guid? UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}

namespace ExpenseApi.Models
{
    public class UpdatePasswordRequestModel
    {
        public string? UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}

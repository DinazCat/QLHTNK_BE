namespace QLHTNK_BE.Models
{
    public class LoginResponseModel
    {
        public int? Id { get; set; }
        public string? AccessToken { get; set; }
        public string? Role { get; set; }
    }
}

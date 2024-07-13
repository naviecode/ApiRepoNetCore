namespace ShopApi.Service.Models.UserDto
{
    public class UserResponse
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? ImageName { get; set; }
        public string? ImageKey { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}

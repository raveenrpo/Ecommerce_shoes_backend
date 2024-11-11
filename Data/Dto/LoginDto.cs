namespace Ecommerse_shoes_backend.Data.Dto
{
    public class LoginDto
    {
        public int Userid { get; set; }
        public string UserName { get; set; }
        //public string Password { get; set; }
        public string Role { get; set; }
        public string Error { get; set; }
        public bool Isblocked { get; set; }

        public string Token { get; set; }
    }
}

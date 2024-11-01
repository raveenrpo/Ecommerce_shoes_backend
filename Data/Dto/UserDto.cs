using System.ComponentModel.DataAnnotations;

namespace Ecommerse_shoes_backend.Data.Dto
{
    public class UserDto
    {
        [Required] 
        public string Name { get; set; }
        public long Phone_no { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace EmpowerIdMicroservice.WebApi.Model
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }

        [Required]
        public string Fullname { get; set; }
    }

    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

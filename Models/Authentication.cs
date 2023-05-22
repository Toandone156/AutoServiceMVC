using System.ComponentModel.DataAnnotations;

namespace AutoServiceMVC.Models
{
    public class Login
    {
        [Required]
        [StringLength(100)]
        [RegularExpression("^(?!.*\\.\\.)(?!.*\\.$)[^\\W][\\w.]{0,29}$", ErrorMessage = "Username is not valid")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[!@#$%^&*()\\-=_+{}[\\]:;|\"',<.>/?]).{8,32}$",
            ErrorMessage = "Password is not valid")]
        public string Password { get; set; }
    }

    public class Register
    {
        [Required]
        [StringLength(100)]
        [RegularExpression("^(?!.*\\.\\.)(?!.*\\.$)[^\\W][\\w.]{0,29}$", ErrorMessage = "Username is not valid")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[!@#$%^&*()\\-=_+{}[\\]:;|\"',<.>/?]).{8,32}$",
                ErrorMessage = "Password is not valid")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[!@#$%^&*()\\-=_+{}[\\]:;|\"',<.>/?]).{8,32}$",
                ErrorMessage = "Password is not valid")]
        public string AgainPassword { get; set; }
        [Required]
        [StringLength(255)]
        public string FullName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNum { get; set; }
        public int RoleId { get; set; }
    }
}

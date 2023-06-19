using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoServiceMVC.Models
{
    public class Login
    {
        [Required]
        [StringLength(100)]
        [RegularExpression("^(?:[\\w.-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}|[\\w]+)$", ErrorMessage = "Username/Email is not valid")]
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
        [Compare("Password", ErrorMessage = "Again password not match")]
        [DisplayName("Re-enter Password")]
        public string AgainPassword { get; set; }
        [Required]
        [StringLength(255)]
        [DisplayName("Full name")]
        public string FullName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [DisplayName("Role")]
        public int? RoleId { get; set; }
    }

    public class ResetPassword
    {
        public int UserId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[!@#$%^&*()\\-=_+{}[\\]:;|\"',<.>/?]).{8,32}$",
                ErrorMessage = "Password is not valid")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Again password not match")]
        [DisplayName("Re-enter Password")]
        public string AgainPassword { get; set; }
    }
}

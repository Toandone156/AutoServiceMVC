using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using AutoServiceMVC.Services.System;
using System.ComponentModel;

namespace AutoServiceMVC.Models
{
    public class Account
    {
        //Common Attributes
        [StringLength(100)]
        [RegularExpression("^(?!.*\\.\\.)(?!.*\\.$)[^\\W][\\w.]{0,100}$", ErrorMessage = "Username is not valid")]
        public string? Username { get; set; }
        [StringLength(255)]
        public string? HashPassword { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Full name")]
		[RegularExpression("^[a-zA-ZÀ-Ỹà-ỹĂăÂâĐđÊêÔôƠơƯư]+(?:\\s+[a-zA-ZÀ-Ỹà-ỹĂăÂâĐđÊêÔôƠơƯư]+)*$", ErrorMessage = "Fullname is not valid")]
		public string FullName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}

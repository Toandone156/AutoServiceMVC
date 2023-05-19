﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceBE.Models
{
    public class Account
    {
        //Common Attributes
        [StringLength(100)]
        [RegularExpression("^(?!.*\\.\\.)(?!.*\\.$)[^\\W][\\w.]{0,29}$", ErrorMessage = "Username is not valid")]
        public string Username { get; set; }
        [StringLength(255)]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[!@#$%^&*()\\-=_+{}[\\]:;|\"',<.>/?]).{8,32}$",
            ErrorMessage = "Password is not valid")]
        [DataType(DataType.Password)]
        public string HashPassword { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string FullName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNum { get; set; }
        [StringLength(255)]
        public string Avatar { get; set; }
    }
}

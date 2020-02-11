using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LoginRegistration.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(255, ErrorMessage="Name may not be more than 255 characters long")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        [MaxLength(255, ErrorMessage="Email may not be more than 255 characters long")]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters in length.")]
        [DataType(DataType.Password)]

        public string Password { get; set; }

        [NotMapped]
        [Required]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]

        public string ConfirmPassword { get; set; }

        public int AccessLevel { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}
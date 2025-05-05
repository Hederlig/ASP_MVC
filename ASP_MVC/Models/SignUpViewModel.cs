using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class SignUpViewModel
    {
    [Required(ErrorMessage = "! Required")]
    [Display(Name = "Full Name", Prompt = "Enter full name")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "! Required")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email")]
    [Display(Name = "Email", Prompt = "Enter email address")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "! Required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Minimum 6 characters, Uppercase, Lowercase & digit")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter password")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "! Required")]
    [Compare(nameof(Password), ErrorMessage = "Password do not match")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password", Prompt = "Confirm password")]
    public string ConfirmPassword { get; set; } = null!;

    [Required]
    [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms")]
    public bool Terms { get; set; }
    }
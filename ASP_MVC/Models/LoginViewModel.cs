using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class LoginViewModel
    {
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email", Prompt = "Enter email address")]
    [Required(ErrorMessage = "! Required")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "! Required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Invalid password")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter password")]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }
    }
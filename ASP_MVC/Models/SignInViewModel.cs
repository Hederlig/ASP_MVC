﻿using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class SignInViewModel
    {
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
    ErrorMessage = "Invalid email address format.")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email", Prompt = "Enter your email")]
    public string Email { get; set; } = null!;

    [Required]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and contain at least one letter and one number.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter password")]

    public string Password { get; set; } = null!;

    [Required]
    [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Comfirm password")]
    public string PasswordConfirmed { get; set; } = null!;

    public bool IsPersistent { get; set; }
    }


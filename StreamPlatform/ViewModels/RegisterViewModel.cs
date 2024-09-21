using System.ComponentModel.DataAnnotations;

namespace StreamPlatform.ViewModels;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "Имя пользователя")]
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Имя пользователя может содержать только буквы и цифры.")]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
}
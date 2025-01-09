using System.ComponentModel.DataAnnotations;

namespace Quiz.ViewModels;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "Username")]
    public string Username { get; set; } = null!;

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = null!;
}

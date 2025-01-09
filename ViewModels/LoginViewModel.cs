using System.ComponentModel.DataAnnotations;

namespace Quiz.ViewModels;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Username")]
    public string Username { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = null!;
}

using System.ComponentModel.DataAnnotations;

public class RegisterUserDTO{
    [Required]
    public string userName { get; set; }

    [Required]
    public string password { get; set; }
}
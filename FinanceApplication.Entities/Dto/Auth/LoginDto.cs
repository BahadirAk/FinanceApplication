using System.ComponentModel.DataAnnotations;

namespace FinanceApplication.Entities.Dto.Auth;

public class LoginDto
{
    [Required]
    public string TaxId { get; set; }
    [Required]
    public string Password { get; set; }
}
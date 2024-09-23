namespace FinanceApplication.Entities.Dto.User;

public class UserCreateDto
{
    public string TaxId { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public byte RoleId { get; set; }   
}
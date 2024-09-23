namespace FinanceApplication.Entities.Dto.User;

public class UserDto
{
    public int Id { get; set; }
    public string TaxId { get; set; }
    public string Name { get; set; }
    public byte RoleId { get; set; }
    public byte[]? PasswordSalt { get; set; } = null;
    public byte[]? PasswordHash { get; set; } = null;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
    public byte Status { get; set; }
}
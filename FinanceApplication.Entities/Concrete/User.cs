namespace FinanceApplication.Entities.Concrete;

public class User : BaseEntity
{
    public int Id { get; set; }
    public string TaxId { get; set; }
    public string Name { get; set; }
    public byte[] PasswordSalt { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte RoleId { get; set; }
    
    public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
}
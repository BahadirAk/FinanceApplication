namespace FinanceApplication.Entities.Concrete;

public class Notification : BaseEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public string Message { get; set; }
}
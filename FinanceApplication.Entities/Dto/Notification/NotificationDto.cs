namespace FinanceApplication.Entities.Dto.Notification;

public class NotificationDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Message { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
    public byte Status { get; set; }
}
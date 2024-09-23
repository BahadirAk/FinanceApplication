using FinanceApplication.Entities.Enums;

namespace FinanceApplication.Entities.Concrete;

public class BaseEntity
{
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
    public byte Status { get; set; } = (byte)StatusEnum.Active;
}
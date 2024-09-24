namespace FinanceApplication.Entities.Dto.Request;

public class RequestDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; }
    public string SupplierTaxId { get; set; }
    public byte RequestStatus { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
    public byte Status { get; set; }
}
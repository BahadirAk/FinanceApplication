namespace FinanceApplication.Entities.Dto.Invoice;

public class InvoiceDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; }
    public string TermDate { get; set; }
    public string BuyerTaxId { get; set; }
    public string SupplierTaxId { get; set; }
    public decimal InvoiceCost { get; set; }
    public byte InvoiceStatus { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
    public byte Status { get; set; }
}
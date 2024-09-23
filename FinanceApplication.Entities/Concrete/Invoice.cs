namespace FinanceApplication.Entities.Concrete;

public class Invoice : BaseEntity
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; }
    public string TermDate { get; set; }
    public string BuyerTaxId { get; set; }
    public string SupplierTaxId { get; set; }
    public decimal InvoiceCost { get; set; }
    public byte InvoiceStatus { get; set; }
}
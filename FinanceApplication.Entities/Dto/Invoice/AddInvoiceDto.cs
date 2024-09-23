namespace FinanceApplication.Entities.Dto.Invoice;

public class AddInvoiceDto
{
    public string InvoiceNumber { get; set; }
    public string TermDate { get; set; }
    public string SupplierTaxId { get; set; }
    public decimal InvoiceCost { get; set; }
}
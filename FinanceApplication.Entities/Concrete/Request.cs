namespace FinanceApplication.Entities.Concrete;

public class Request : BaseEntity
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; }
    public string SupplierTaxId { get; set; }
    public byte RequestStatus { get; set; }
}
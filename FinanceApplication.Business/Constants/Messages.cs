namespace FinanceApplication.Business.Constants;

public static class Messages
{
    public static string Success = "İşleminiz başaıryla gerçekleşti.";
    public static string UnknownError = "Bilinmeyen bir hata meydana geldi.";
    public static string DataExists = "Kayıt sistemde mevcut.";
    public static string AddFailed = "Ekleme sırasında bir hata meydana geldi.";
    public static string UpdateFailed = "Güncelleme sırasında bir hata meydana geldi.";
    public static string DataNotFound = "Veri bulunamadı.";
    public static string UserLoginError = "Lütfen girdiğiniz bilgileri kontrol ediniz.";
    public static string TokenError = "Geçersiz token";
    public static string InvoiceSuccess = "{0} numaralı fatura/faturalar zaten sistemde bulunmaktadır. Diğer faturanız/faturalarınız işleme alınmıştır.";
    public static string InvoiceSupplierSuccess = "{0} numaralı tedarikçi/tedarikçiler sistemde bulunmadığından onlara ait faturalar eklenemedi.";
    public static string SameInvoiceNumber = "Aynı fatura numarasından birden fazla olamaz.";
    public static string IsWaitingRequest = "{0} numaralı faturaya ait onay bekleyen talebiniz bulunmaktadır.";
    public static string ApprovedRequest = "{0} numaralı faturaya ait talebiniz zaten onaylanmış.";
    public static string BuyerRequest = "{0} numaralı faturanız kullanılmıştır. Yakın zamanda ödemeniz gerçekleşecektir.";
    public static string ApprovedRequestSupplier = "{0} numaralı faturanız için açıtığınız talep onaylanmıştır.";
    public static string ApprovedRequestBuyer = "{0} numaralı faturanız için ödemeniz gerçekleşmiştir.";
}
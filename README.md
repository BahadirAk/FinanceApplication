- Proje 11 adet endpointten oluşmaktadır. Login dışındaki 10 endpointi kullanabilmek için Admin/Financial Institution/Buyer/Supplier yetkilerinden birine sahip bir hesap ile giriş yapılmalıdır.
- Proje ayağa kaldırıldığında Admin ve Financial Institution yetkisine sahip 2 adet hesap database tarafına "Seed Data" olarak kayıt edilmektedir. Bu hesao bilgileri:

  ADMIN
  TaxId: 1122334455
  Password: admin123

  Financial Instituion
  TaxId: 1234567890
  Password: finance123

- Invoice Post: "Buyer" yetkisine sahip kullanıcı sisteme birden fazla fatura ekleyebilir. Fatura eklenen tedarikçilere bildirim gönderilir.
- Invoice Get: "Supplier" yetkisine sahip kullanıcı kendisine gönderilen faturaları listeleyebilir.
- Invoice Put: "Buyer" yetkisine sahip kullanıcı "New" durumunda olan faturasını iptal edebilir.

- Request Post: "Supplier" yetkisine sahip kullanıcı kendisine gönderilen faturalar arasından seçtiği fatura için erken ödeme talebinde bulunabilir. Faturayı oluşturan alıcıya bildirim gönderilir.
- Request Get: "Financial Institution" yetkisine sahip kullanıcı, açılan erken ödeme taleplerini listeleyebilir.
- Request Put: "Financial Institution" yetkisine sahip kullanıcı açılan erken ödeme talepleri arasından seçtiği talebi onaylayabilir. Erken ödeme talebi açan tedarikçiye ve faturayı ekleyen alıcıya bildirim gönderilir.

- Users Get (getusers): "Admin" yetkisine sahip kullanıcı sisteme kayıtlı kullanıcıları listeleyebilir.
- Users Post (addbuyer): "Admin" yetkisine sahip kullanıcı sisteme alıcı ekleyebilir.
- Users Post (addsupplier): "Admin" yetkisine sahip kullanıcı sisteme tedarikçi ekleyebilir.
- Users Get (notifications): "Buyer" / "Supplier" yetkisine sahip alıcı/tedarikçi kendisine gönderilen bildirimleri listeleyebilir.

Sistemi ayağa kaldırmak için kullanılabilecek kodlar:

docker build -t finance-app.api -f FinanceApplication.API/Dockerfile .
docker-compose up

Sistem ayağa kalktıktan sonra "http://localhost:5002/swagger/index.html" adresinden Swagger'a erişebilirsiniz.

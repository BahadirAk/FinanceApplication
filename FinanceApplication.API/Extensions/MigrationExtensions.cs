using FinanceApplication.Dal.Concrete.Context;
using FinanceApplication.Entities.Concrete;
using FinanceApplication.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceApplication.API.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            using (var context = scope.ServiceProvider.GetRequiredService<FinanceAppDbContext>())
            {
                context.Database.Migrate();
                
                if(context.Users.Any()) return;

                var users = new User[]
                {
                    new User
                    {
                        TaxId = "", Name = "Admin", PasswordSalt = "0xF48F8298B9B3687DEC43A3ED6156745EF7D48D01822AA78E29B4C08F5E546DF5E77E0E576826B096380FD54CA3CA5F82635AAB980284AB51348009B77414E3CC9C456F483D7C353711F5C88DCE82B5531DA98548179B24685782CFEC8942A75CE7112F315FA7AFB9B0DA016DA5C5AABF8D2E3073F60453E600BABAE40FF5961A".HexStringToByteArray(), PasswordHash = "0x78235D644377AFB7F5FAF83064F6B78622D425EFDBB37D0C7CE8E9D8A1C5BAD2C3E337DB0344A41B9C276EBCCADEF78005F635F3CFC94464B492B9176EE554B0".HexStringToByteArray(), RoleId = (byte)RoleEnum.Admin, CreatedDate = DateTime.UtcNow, Status = (byte)StatusEnum.Active
                    },
                    new User
                    {
                        TaxId = "123456789", Name = "Financial Institution", PasswordSalt = "0x21FA78B4FD72FD4FCE584AD92F0235AD694CFF00C15430D0396D9E0F7AA6EFC1C29083DA8520E224778FE1AC3F6F1EFA2A4A81DBC266C40D060967635EB40C7BCA5202ADE8D9F186EC36232105CFC65A113FA92EEAF5C91BAF43A30D95221825D4FF890F25F448B591918129C6C52E85657867215A76807C1758C0380D2B58AE".HexStringToByteArray(), PasswordHash = "0x28CCC65CBEE3B3E12CDB67DF6DB3FEABC6D3B205C5B874B3804AEE3343AC67F457E8074CD689EF1A281C44AE3EA978F8D368F860D9F7FDC83DD16FF79B18798E".HexStringToByteArray(), RoleId = (byte)RoleEnum.FinancialInstitution, CreatedDate = DateTime.UtcNow, Status = (byte)StatusEnum.Active
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
    
    public static byte[] HexStringToByteArray(this string hex)
    {
        if (hex.StartsWith("0x"))
        {
            hex = hex.Substring(2);
        }

        if (hex.Length % 2 != 0)
        {
            throw new ArgumentException("Hex string must have an even length");
        }

        byte[] bytes = new byte[hex.Length / 2];
        for (int i = 0; i < hex.Length; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }
}
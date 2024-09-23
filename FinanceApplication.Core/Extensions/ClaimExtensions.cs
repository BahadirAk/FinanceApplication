using System.Security.Claims;

namespace FinanceApplication.Core.Extensions;

public static class ClaimExtensions
{
    public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
    {
        claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
    }

    public static void AddRoles(this ICollection<Claim> claims, byte[] roles)
    {
        roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role.ToString())));
    }
    
    public static void AddTaxId(this ICollection<Claim> claims, string taxId)
    {
        claims.Add(new Claim(ClaimTypes.UserData, taxId));
    }
}
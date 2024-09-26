using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceApplication.Core.Security;

public static class UserIdentityHelper
{
    private static IHttpContextAccessor _httpContextAccessor = ServiceTool.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

    public static void SetUserInfo(string userId, string roleId, string taxId)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Role, roleId),
            new Claim(ClaimTypes.UserData, taxId)
        };

        var identity = new ClaimsIdentity(claims, "custom");
        var principal = new ClaimsPrincipal(identity);

        _httpContextAccessor.HttpContext.User = principal;
    }

    public static int GetUserId()
    {
        return int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    }
    
    public static string GetUserTaxId()
    {
        return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.UserData)?.Value;
    }

    public static byte GetUserRoleId()
    {
        return byte.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value);
    }
}
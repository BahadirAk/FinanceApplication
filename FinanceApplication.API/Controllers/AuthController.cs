using FinanceApplication.Business.Abstract;
using FinanceApplication.Entities.Dto.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FinanceApplication.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [SwaggerOperation(Summary = "Giriş yap.", Description = "Kullanıcı vergi numarasını ve şifresini girerek sisteme giriş yapar.")]
    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login(LoginDto loginDto)
    {
        var result = _authService.Login(loginDto);  
        return StatusCode(result.HttpStatusCode, result);
    }
}
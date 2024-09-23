using System.Net;
using FinanceApplication.Business.Abstract;
using FinanceApplication.Core.Result;
using FinanceApplication.Entities.Dto.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    
    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login(LoginDto loginDto)
    {
        var result = _authService.Login(loginDto);  
        return StatusCode(result.HttpStatusCode, result);
    }
}
using FinanceApplication.Business.Abstract;
using FinanceApplication.Entities.Dto.User;
using FinanceApplication.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FinanceApplication.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _usersService;
    public UsersController(IUserService usersService)
    {
        _usersService = usersService;
    }
    
    [SwaggerOperation(Summary = "Kullanıcıları listeler.", Description = "Admin yetkisine sahip kişi sistemdeki kullanıcıları görüntüler.")]
    [Authorize(Roles = "1")]
    [HttpGet("getusers")]
    public IActionResult Get()
    {
        var result = _usersService.GetList();
        return StatusCode(result.HttpStatusCode, result);
    }

    [SwaggerOperation(Summary = "Alıcı ekleme.", Description = "Admin yetkisine sahip kişi sisteme alıcı ekler.")]
    [Authorize(Roles = "1")]
    [HttpPost("addbuyer")]
    public IActionResult AddBuyer(UserCreateDto userCreateDto)
    {
        userCreateDto.RoleId = (byte)RoleEnum.Buyer;
        
        var result = _usersService.Add(userCreateDto);
        return StatusCode(result.HttpStatusCode, result);
    }
    
    [SwaggerOperation(Summary = "Tedarikçi ekleme.", Description = "Admin yetkisine sahip kişi sisteme tedarikçi ekler.")]
    [Authorize(Roles = "1")]
    [HttpPost("addsupplier")]
    public IActionResult AddSupplier(UserCreateDto userCreateDto)
    {
        userCreateDto.RoleId = (byte)RoleEnum.Supplier;
        
        var result = _usersService.Add(userCreateDto);
        return StatusCode(result.HttpStatusCode, result);
    }


    [SwaggerOperation(Summary = "Bildirim listeleme.", Description = "Alıcı ve tedarikçi yetkilerine sahip kişiler kendilerine ait bildirimleri görüntüleyebilirler.")]
    [Authorize(Roles = "2,3")]
    [HttpGet("notifications")]
    public IActionResult GetUserNotifications()
    {
        var result = _usersService.GetUserNotifications();
        return StatusCode(result.HttpStatusCode, result);
    }
}
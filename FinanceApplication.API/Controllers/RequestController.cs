using FinanceApplication.Business.Abstract;
using FinanceApplication.Entities.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FinanceApplication.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RequestController : ControllerBase
{
    private readonly IRequestService _requestService;

    public RequestController(IRequestService requestService)
    {
        _requestService = requestService;
    }

    [SwaggerOperation(Summary = "Erken ödeme talebi oluşturma.", Description = "Tedarikçi yetkisine sahip kişi erken ödeme talebi oluşturur.")]
    [Authorize(Roles = "3")]
    [HttpPost]
    public IActionResult AddRequest(AddRequestDto addRequestDto)
    {
        var result = _requestService.AddRequest(addRequestDto);
        return StatusCode(result.HttpStatusCode, result);
    }

    [SwaggerOperation(Summary = "Erken ödeme taleplerini görüntüleme.", Description = "Finans kurumu yetkisine sahip kişi erken ödeme taleplerini görüntüler.")]
    [Authorize(Roles = "4")]
    [HttpGet]
    public IActionResult GetList()
    {
        var result = _requestService.GetList();
        return StatusCode(result.HttpStatusCode, result);
    }

    [SwaggerOperation(Summary = "Erken ödeme talebi onaylama.", Description = "Finans kurumu yetkisine sahip kişi erken ödeme talebini onaylar.")]
    [Authorize(Roles = "4")]
    [HttpPut]
    public IActionResult ApproveRequest(int id)
    {
        var result = _requestService.ApproveRequest(id);
        return StatusCode(result.HttpStatusCode, result);
    }
}
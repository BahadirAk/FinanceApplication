using FinanceApplication.Business.Abstract;
using FinanceApplication.Entities.Dto.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FinanceApplication.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [SwaggerOperation(Summary = "Fatura ekleme.", Description = "Alıcı yetkisine sahip kişi sisteme faturalarını ekler.")]
    [Authorize(Roles = "2")]
    [HttpPost]
    public IActionResult AddInvoice(List<AddInvoiceDto> addInvoiceDto)
    {
        var result = _invoiceService.Add(addInvoiceDto);
        return StatusCode(result.HttpStatusCode, result);
    }

    [SwaggerOperation(Summary = "Fatura listeleme.", Description = "Tedarikçi yetkisine sahip kişi kendisine atanan faturaları görüntüler.")]
    [Authorize(Roles = "3")]
    [HttpGet]
    public IActionResult GetList()
    {
        var result = _invoiceService.GetList();
        return StatusCode(result.HttpStatusCode, result);
    }

    [SwaggerOperation(Summary = "Fatura iptal.", Description = "Alıcı yetkisine sahip kişi faturasını iptal eder.")]
    [Authorize(Roles = "2")]
    [HttpPut]
    public IActionResult Backout([FromQuery]string invoiceNumber)
    {
        var result = _invoiceService.Backout(invoiceNumber);
        return StatusCode(result.HttpStatusCode, result);
    }
}
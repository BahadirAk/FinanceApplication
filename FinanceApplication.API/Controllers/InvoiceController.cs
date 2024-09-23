using FinanceApplication.Business.Abstract;
using FinanceApplication.Entities.Dto.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [Authorize(Roles = "2")]
    [HttpPost]
    public IActionResult AddInvoice(List<AddInvoiceDto> addInvoiceDto)
    {
        var result = _invoiceService.Add(addInvoiceDto);
        return StatusCode(result.HttpStatusCode, result);
    }
}
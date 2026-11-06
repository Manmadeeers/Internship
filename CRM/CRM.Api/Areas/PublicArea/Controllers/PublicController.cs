using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Api.Areas.PublicArea.Controllers;

[Area("Public")]
[DisplayName("Public Controller")]
[Route("api/[area]/[controller]")]
[ApiController]
public class PublicController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Response from public area");
    }
}
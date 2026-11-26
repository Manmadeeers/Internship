using System.ComponentModel;
using CRM.Service.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Api.Areas.Customers
{
    [Area("Customers")]
    [Route("api/[area]/[controller]")]
    [DisplayName("Customers controller")]
    [ApiController]
    [Authorize]
    public class CustomersController(ICustomerService customerService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var response = await customerService.GetAllCustomersAsync();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var response = await customerService.GetCustomerByIdAsync(id);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        
        
    }
}

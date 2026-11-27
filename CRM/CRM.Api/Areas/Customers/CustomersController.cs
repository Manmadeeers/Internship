using System.ComponentModel;
using System.Net;
using CRM.Model.InputModels.Customers;
using CRM.Service.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Api.Areas.Customers
{
    [Area("Customers")]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> GetCustomer(Guid id)
        {
            if (!Guid.TryParse(id.ToString(), out Guid customerId))
            {
                return BadRequest();
            }
            
            var response = await customerService.GetCustomerByIdAsync(customerId);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody]CustomerCreateInputModel model)
        {
            var response = await customerService.CreateCustomer(model);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody]CustomerUpdateInputModel model)
        {
            var response = await customerService.UpdateCustomer(id, model);
            if (response.IsSuccess)
            {
                return Ok(response);
            }

            if (response.Data == HttpStatusCode.Conflict)
            {
                return Conflict(response);
            }

            if (response.Data == HttpStatusCode.NotFound)
            {
                return NotFound(response);
            }
            
            return BadRequest(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var response = await customerService.DeleteCustomer(id);
            
            return response.IsSuccess ? NoContent() : NotFound();
        }
        
        
    }
}

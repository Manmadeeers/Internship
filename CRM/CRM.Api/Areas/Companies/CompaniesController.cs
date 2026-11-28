using System.ComponentModel;
using System.Net;
using CRM.Model.InputModels.Companies;
using CRM.Service.Companies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Api.Areas.Companies
{
    [Area("Companies")]
    [DisplayName("Companies controller")]
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class CompaniesController(ICompaniesService companiesService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var response = await companiesService.GetAllCompaniesAsync();
            return response.IsSuccess ? Ok(response) : NoContent();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCompanyById(Guid id)
        {
            var response = await companiesService.GetCompanyByIdAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany([FromBody] CompanyCreateInputModel model)
        {
            var response = await companiesService.CreateCompanyAsync(model);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyUpdateInputModel model)
        {
            var response = await companiesService.UpdateCompanyAsync(id, model);
            if (response.Data == HttpStatusCode.NotFound)
            {
                return NotFound(response);
            }
            if (response.Data == HttpStatusCode.Conflict)
            {
                return Conflict();
            }
            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            var response = await companiesService.DeleteCompanyAsync(id);
            return response.IsSuccess ? NoContent() : NotFound(response);
        }
    }
}

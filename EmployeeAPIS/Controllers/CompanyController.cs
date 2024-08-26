using EmployeeAPIS.Services;
using EmployeesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPIS.Controllers;

[Route("api/[controller]")]
[ApiController]

public class CompanyController(ICompanyService companyService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompanyMod>>> GetAllCompanies()
    {
        var companies = await companyService.GetAllCompaniesAsync();
        return Ok(companies);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Company>> GetCompany(int id)
    {
        var company = await companyService.GetCompanyByIdAsync(id);
        if (company == null)
        {
            return NotFound();
        }

        return Ok(company);
    }
    
    [HttpPost]
    public async Task<ActionResult<CompanyMod>> AddCompany(CompanyMod companyMod)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var company = new Company(companyMod.Name);
        var addedCompany = await companyService.AddCompanyAsync(company);

        var compMod = new CompanyMod
        {
            Id = addedCompany.CompanyId,
            Name = addedCompany.Name
        };

        return CreatedAtAction(nameof(GetCompany), new { id = compMod.Id }, compMod);
    }

    [HttpPost("AddMany")]
    public async Task<ActionResult<IEnumerable<CompanyMod>>> AddSeveralCompanies(IEnumerable<CompanyMod> companyMods)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var companies = companyMods.Select(cm => new Company(cm.Name)).ToList();
        
        
        var addedCompanies = await companyService.AddManyCompaniesAsync(companies);
        var newCompanies = addedCompanies.Select(c => new CompanyMod
        {
            Id = c.CompanyId,
            Name = c.Name
        }).ToList();
        return CreatedAtAction(nameof(AddSeveralCompanies), new { }, newCompanies);
       
    }
    [HttpPut]
    public async Task<IActionResult> UpdateCompany(int id, [FromBody] string newName)
    {
        if (newName==null)
        {
            return BadRequest("Company name is required.");
        }
        try
        {
            await companyService.UpdateCompanyAsync(id, newName);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        var existingCompany = await companyService.GetCompanyByIdAsync(id);
        if (existingCompany == null) return NotFound();
        await companyService.DeleteCompanyAsync(id);
        return NoContent();
    }
}
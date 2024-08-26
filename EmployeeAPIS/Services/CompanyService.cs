using EmployeeAPIS.Controllers;
using EmployeeAPIS.Data;
using EmployeesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPIS.Services;

public class CompanyService : ICompanyService
{
    private readonly ApplicationDbContext _context;

    public CompanyService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<CompanyModel>> GetAllCompaniesAsync()
    {
        return await _context.Companies.Include(c =>c.Employees).Select(c => new CompanyModel
        {
            Id = c.CompanyId,
            Name = c.Name,
            Employees = c.Employees.Select(e => new EmployeeModel
            {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                Mobile = e.Mobile,
                CompanyId= c.CompanyId,
                Company = e.Company
            }).ToList()
        }).ToListAsync();
    }
    public async Task<CompanyModel> GetCompanyByIdAsync(int id)
    {
        var company = await _context.Companies
            .Include(c => c.Employees)
            .Where(c => c.CompanyId == id)
            .Select(c => new CompanyModel
            {
                Id = c.CompanyId,
                Name = c.Name,
                Employees = c.Employees.Select(e => new EmployeeModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    Mobile = e.Mobile,
                    CompanyId = c.CompanyId,
                    Company = e.Company
                    
                }).ToList()
            })
            .SingleOrDefaultAsync();

        return company;
    }
    public async Task<Company> AddCompanyAsync(Company company)
    {
        var existingCompany = await _context.Companies
            .FirstOrDefaultAsync(c => c.Name == company.Name);
    
        if (existingCompany != null)
        {
            throw new InvalidOperationException("A company with the same name already exists.");
        }
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
        return company;
    }
    public async Task<IEnumerable<Company>> AddManyCompaniesAsync(IEnumerable<Company> companies)
    {
        var companyList = companies.ToList();
        var existingCompaniesNames = await _context.Companies
            .Where(c => companyList.Select(x => x.Name).Contains(c.Name))
            .Select(c => c.Name)
            .ToListAsync();
        
        var newCompanies = companyList
            .Where(c => !existingCompaniesNames.Contains(c.Name))
            .ToList();
       
        _context.Companies.AddRange(newCompanies);
        await _context.SaveChangesAsync();
        return newCompanies;
       
    }
    public async Task UpdateCompanyAsync(int id, string newName)
    {
        var existingCompany = await _context.Companies.FindAsync(id);
        if (existingCompany == null)
        {
            throw new KeyNotFoundException("No company has this Id.");
        }
        existingCompany.Name = newName;
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteCompanyAsync(int id)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company != null)
        {
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new KeyNotFoundException("No company is found with this ID.");
        }
    }
}
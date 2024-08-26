using EmployeesAPI.Models;

namespace EmployeeAPIS.Services;

public interface ICompanyService
{
    Task<IEnumerable<CompanyModel>> GetAllCompaniesAsync();
    Task<CompanyModel> GetCompanyByIdAsync(int id);
    Task<Company> AddCompanyAsync(Company company);
    Task<IEnumerable<Company>> AddManyCompaniesAsync(IEnumerable<Company> companies);
    Task UpdateCompanyAsync(int id, string newName);
    Task DeleteCompanyAsync(int id);
}
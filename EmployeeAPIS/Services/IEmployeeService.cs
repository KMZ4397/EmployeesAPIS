using EmployeesAPI.Models;
using EmployeesAPI.Services;

namespace EmployeeAPIS.Services;

public interface IEmployeeService
{
    Task<List<EmployeeMod>> GetAllEmployeesAsync();
    Task<EmployeeMod> GetEmployeeByIdAsync(int id);
    Task<Employee> AddEmployeeAsync(CreateEmployeeReq request);
    Task UpdateEmployeeAsync(int id,EmployeeMod employee);
    Task DeleteEmployeeAsync(int id);
}
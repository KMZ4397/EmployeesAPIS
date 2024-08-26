using System.Net;
using EmployeeAPIS.Controllers;
using EmployeeAPIS.Data;
using EmployeesAPI.Models;
using Microsoft.EntityFrameworkCore;
using EmployeeAPIS.Services;

namespace EmployeesAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private IEmployeeService _employeeServiceImplementation;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeMod>> GetAllEmployeesAsync()
        {
            return await _context.Employees.Include(e => e.Company).Select(e => new EmployeeMod
            {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                Mobile = e.Mobile,
                CompanyId = e.CompanyId,
                company = e.Company
            }).ToListAsync();
        }

        public async Task<EmployeeMod> GetEmployeeByIdAsync(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Company)
                .Where(e => e.Id == id)
                .Select(e => new EmployeeMod
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    Mobile = e.Mobile,
                    CompanyId = e.CompanyId,
                    company = e.Company
                }).SingleOrDefaultAsync();
            return employee;
        }

        public async Task<Employee> AddEmployeeAsync(CreateEmployeeReq request)
        {
            bool validCompany = await _context.Companies.AnyAsync(c => c.CompanyId==request.CompanyId);
            if (!validCompany)
            {
                throw new KeyNotFoundException("No company is under the Id.");
            }
            var employee = new Employee(request.Name, request.Email, request.Mobile, request.CompanyId);
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task UpdateEmployeeAsync(int id, EmployeeMod employee)
        {
            var existingEmployee = await _context.Employees.FindAsync(id);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException("No employee has this Id.");
            }

            existingEmployee.Name = employee.Name;
            existingEmployee.Email = employee.Email;
            existingEmployee.Mobile = employee.Mobile;
            existingEmployee.CompanyId = employee.CompanyId;

            await _context.SaveChangesAsync();
        }


        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }
    }
}

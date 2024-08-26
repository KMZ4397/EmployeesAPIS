using System.ComponentModel.DataAnnotations;
using EmployeeAPIS.Services;

namespace EmployeesAPI.Models;

public class Company
{
    private Company()
    {
        Employees = new List<Employee>();
    }

    public Company(string name)
    {
        Name = name;
        Employees = new List<Employee>();
    }
    [Required]
    public int CompanyId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    public IEnumerable<Employee> Employees { get; set; } = new List<Employee>();
}
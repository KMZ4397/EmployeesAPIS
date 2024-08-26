using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeesAPI.Models;

public class CreateEmployeeRequest
{
    public string Name { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    [Phone]
    public string Mobile { get; set; }
    [Required]
    public int CompanyId { get; set; }
    public Company Company { get; set; }
}
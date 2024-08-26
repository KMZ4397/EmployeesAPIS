using System.ComponentModel.DataAnnotations;

namespace EmployeesAPI.Models;

public class CreateEmployeeReq
{
    public string Name { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    [Phone]
    public string Mobile { get; set; }
    [Required]
    public int CompanyId { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EmployeesAPI.Models
{
    public class Employee
    {
        private Employee()
        {
            
        }
        public Employee(string name, string email, string mobile, int companyId, Company company)
        {
            Name = name;
            Email = email;
            Mobile = mobile;
            CompanyId = companyId;
            Company = company;
        }

        public Employee(string requestName, string requestEmail, string requestMobile, int requestCompanyId)
        {
            Name = requestName;
            Email = requestEmail;
            Mobile = requestMobile;
            CompanyId = requestCompanyId;
        }
        [Required]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Mobile { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public Company Company { get; set; }

    }
}

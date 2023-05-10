using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMtool.Application.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string OfficeEmail { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string PersonalEmail { get; set; }
        [Required]
        public bool EmployeeType { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        public int Experience { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        [Phone]
        public long PhoneNumber { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BMtool.Core.Entities
{
    public class UpdateDto
    {
        //[Key]
        //public int Id { get; set; }
        //[Required]
        //[Display(Name = "First Name")]
        //public string FirstName { get; set; }
        //[Required]
        //[Display(Name = "Last Name")]
        //public string LastName { get; set; }
        //[Required]
        //[DataType(DataType.EmailAddress)]
        //public string Email { get; set; }
        //[Required]
        //public string type { get; set; }
        //[Required]
        //public string Department { get; set; }
        //[Required]
        //public double Experience { get; set; }
        //[Required]
        //public string Role { get; set; }
        //[Required]
        //[Display(Name = "Phone Number")]
        //public long PhoneNumber { get; set; }

        //[Key]
        //public int Id { get; set; }
        //[Required]
        //[Display(Name = "First Name")]
        //public string FirstName { get; set; }
        //[Required]
        //[Display(Name = "Last Name")]
        //public string LastName { get; set; }
        //[Required]
        //[DataType(DataType.EmailAddress)]
        //public string Email { get; set; }
        //[Required]
        //public string type { get; set; }
        //[Required]
        //public string Department { get; set; }
        //[Required]
        //public double Experience { get; set; }
        //[Required]
        //public string Role { get; set; }
        //[Required]
        //[Display(Name = "Phone Number")]
        //public string PhoneNumber { get; set; }

        //UpdateDto

        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FName { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Last Name")]
        public string LName { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string OfficeEmail { get; set; }= string.Empty;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string PersonalEmail { get; set; } =string.Empty;
        [Required]
        public bool EmployeeType { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        public int Experience { get; set; }
        //[Required]
        //public string Role { get; set; }
        [Required][Display(Name = "Phone Number")] public string Phone { get; set; } = string.Empty;
    }
}

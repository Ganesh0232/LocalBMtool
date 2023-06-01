using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMtool.Application.Models
{
    public class ImportModelclass
    {
        public int ID { get; set; }
        public string EmployeeNumber { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string Phone { get; set; }
        public string OfficeEmail { get; set; }
        public string Mobile { get; set; }
        public string PersonalEmail { get; set; }
        public bool EmployeeType { get; set; }
        public int Experience { get; set; }
        public int DesignationId { get; set; }
        public bool IsInProject { get; set; }
        public bool IsUpskilling { get; set; }
        public bool IsWorkingOnInternalTool { get; set; }
        public int ProjectId { get; set; }
        public int NotesId { get; set; }
        public bool IsFirstLogin { get; set; }
        public int DeptId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime JoinedOn { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; }
     
    }
}

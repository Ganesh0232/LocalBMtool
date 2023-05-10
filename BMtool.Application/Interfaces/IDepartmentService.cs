using BMtool.Application.Models;
using BMtool.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMtool.Application.Interfaces
{
    public interface IDepartmentService
    {
        public List<DepartmentModel> GetDepartmentList();

        // G232
        List<RegisterModel> GetAll();
        List<RegisterModel> GetUser(int id);
        List<RegisterModel> Register(UpdateDto model);

        List<RegisterModel> UpdatedList(int id, UpdateDto model);
    }
}

﻿using BMtool.Application.Models;
using BMtool.Core.Entities;

namespace BMtool.Application.Interfaces
{
    public interface IDepartmentService
    {
        public List<DepartmentModel> GetDepartmentList();

        // G232
        List<RegisterModel> GetAll();
        List<RegisterModel> GetUser(int id);
        List<RegisterModel> Register(UpdateDto model);

        void UpdatedList(int id, UpdateDto model);
        void UpdatedListUsingStoredProc(int id, UpdateDto model);

        void ExcelExpo(string fileName);
        void ImportFile();
    }
}

using AutoMapper;
using BMtool.Application.Interfaces;
using BMtool.Application.Models;
using BMtool.Core.Entities;
using BMtool.Core.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BMtool.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            this._departmentRepository = departmentRepository;
            this._mapper = mapper;
        }

        public List<RegisterModel> GetUser(int id)
        {
            var departmentDto = _departmentRepository.GetRegisteredUserById(id);
            var departmentModel = _mapper.Map<List<RegisterModel>>(departmentDto);
            return departmentModel;
        }

        public List<RegisterModel> GetAll()
        {
            var Dto = _departmentRepository.GetAllRegisteredUsers();
            var DM = _mapper.Map<List<RegisterModel>>(Dto);
            return DM;
        }

        public List<DepartmentModel> GetDepartmentList()
        {
            var departmentdto = _departmentRepository.GetAllDepartmentListAsync();
            var departmentmodel = _mapper.Map<List<DepartmentModel>>(departmentdto);
            return departmentmodel;
        }

        public void UpdatedList(int id, UpdateDto model)
        {
            _departmentRepository.UpdateRegisteredUserAsync(id, model);
        }





        public List<RegisterModel> Register(UpdateDto model)
        {
            var departmentdto = _departmentRepository.RegisterAsync(model);
            var departmentmodel = _mapper.Map<List<RegisterModel>>(departmentdto);
            return departmentmodel;
        }

        public void UpdatedListUsingStoredProc(int id, UpdateDto model)
        {
            _departmentRepository.UpdateRegisteredUserAsyncUsingStoredProc(id, model);
        }

        public void ExcelExpo(string fileName)
        {
            _departmentRepository.Excelfile(fileName);
        }



        //public List<RegisterModel> Register(Register model)
        //{
        //    var departmentDto = _departmentRepository.RegisterAsync(model);
        //    var departmentModel = _mapper.Map<List<RegisterModel>>(departmentDto);

        //    return departmentModel.ToList();
        //}


        /// <summary>
        /// Retrieves a list of all departments from the repository and maps them to a list of DepartmentModel objects.
        /// </summary>
        /// <returns>A list of DepartmentModel objects representing the departments.</returns>


    }
}

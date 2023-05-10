using BMtool.Application.Interfaces;
using BMtool.Application.Models;
using BMtool.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BMtool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            this._departmentService = departmentService ?? throw new ArgumentNullException(nameof(departmentService));
        }
        /// <summary>
        /// Retrieves a list of departments from the database.
        /// </summary>
        /// <returns>A list of DepartmentModel objects representing the departments.</returns>
        [HttpGet]
        public List<DepartmentModel> GetDepartment()
        {
            try
            {
                var departments = _departmentService.GetDepartmentList();
                return departments;
            }
            catch (Exception ex)
            {
                // Log the exception somewhere for debugging purposes
                Console.WriteLine($"An error occurred while processing your request: {ex.Message}");
                throw;
            }

        }
        [HttpGet("GetUsers")]
        public List<RegisterModel> GetUser()
        {
            try
            {
                var departments = _departmentService.GetAll();
                return departments;
            }
            catch (Exception ex)
            {
                // Log the exception somewhere for debugging purposes
                Console.WriteLine($"An error occurred while processing your request: {ex.Message}");
                throw;
            }

        }

        [HttpPut("UpdateUser")]

        public IActionResult UpdateUser(int id, UpdateDto model)
        {
            try
            {
                _departmentService.UpdatedList(id, model);

                return Ok(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            //try
            //{
            //    var user = _departmentService.GetUser(id);

            //    if (user != null)
            //    {
            // var userupdate = _departmentService.UpdatedList(id, model);

            //    return userupdate;
            //}


            //}

            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            //var departments = _departmentService.GetAll();
            //return departments;
        }

        [HttpPut("UpdateUserUsingStoredProc")]

        public IActionResult UpdateUserUsingStoredProc(int id, UpdateDto model)
        {
            try
            {
                _departmentService.UpdatedListUsingStoredProc(id, model);
                return Ok(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

            [HttpPost("CreateUser")]

        public List<RegisterModel> CreateUser([FromBody] UpdateDto model)
        {
            try
            {
                var user = _departmentService.Register(model);

                var departments = _departmentService.GetAll();
                return departments;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}

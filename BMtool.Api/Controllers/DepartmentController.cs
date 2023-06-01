using BMtool.Application.Interfaces;
using BMtool.Application.Models;
using BMtool.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

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
        [HttpPost("Excel")]

        public IActionResult DExcel(string fileName)
        {
            _departmentService.ExcelExpo(fileName);

            return Ok();
        }

        [HttpPost("ExcelImport")]

        public IActionResult ImportExcel()
        {
            try
            {

                _departmentService.ImportFile();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }



        }

         [HttpGet("IST")]
        public DateTime GetIST()
        {

            // Get the current UTC time
            DateTime utcTime = DateTime.UtcNow;



            // Specify the Indian Standard Time zone
            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");



            // Convert UTC time to IST
            DateTime istTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, istZone);



            Console.WriteLine("Current time in IST: " + istTime.ToString("yyyy-MM-dd HH:mm:ss"));

            //  return Ok( istTime.ToString("yyyy-MM-dd HH:mm:ss"));
            return istTime;


        }
        //public string GetToken()
        //{
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var token = new JwtSecurityToken(
        //    _configuration["Jwt:Issuer"],
        //    _configuration["Jwt:Audience"],
        //    expires: GetIST().AddSeconds(10),
        //    signingCredentials: credentials
        //    );
        //    //var tokenInfo = new TokenInfo
        //    //{
        //    //    Token = new JwtSecurityTokenHandler().WriteToken(token),
        //    //   // ValidTo = token.ValidTo
        //    //};



        //    //return tokenInfo;
        //    // return token.ValidTo;
        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

    }
}

using BMtool.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using BMtool.Infrastructure.Data.Context;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DataTable = System.Data.DataTable;
using Workbook = DocumentFormat.OpenXml.Spreadsheet.Workbook;
using Worksheet = DocumentFormat.OpenXml.Spreadsheet.Worksheet;
using Sheets = DocumentFormat.OpenXml.Spreadsheet.Sheets;

namespace BMtool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserStoryController : ControllerBase
    {
        private readonly IUserStoriesRepo repo;
        private readonly BMtoolContext _context;
        private readonly object connection;

        public UserStoryController(IUserStoriesRepo repo, BMtoolContext context)
        {
            this.repo = repo;
            this._context = context;
            this.connection = _context.CreateConnection();
        }
        //[HttpGet("GetUsers")]
        //public IActionResult Get()
        //{
        //    return Ok(repo.GetAll());
        //}

        //[HttpPut("updateUser")]
        //public Task<List<Register>> Update (int id , Register model)
        //{

        //    return repo.UpdateUser(id, model);

        //}

        [HttpPost("ExcelFileExporter")]

        public IActionResult ExcelStore()
        {
            return Ok();
        }




        }
    }

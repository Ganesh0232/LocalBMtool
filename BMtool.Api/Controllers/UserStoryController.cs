//using BMtool.Application.Interfaces;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using BMtool.Core.Repository;
//using BMtool.Application.Interfaces;
//using BMtool.Application.Models;
//using BMtool.Core.Entities;

//namespace BMtool.Api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UserStoryController : ControllerBase
//    {
//        private readonly IUserStoriesRepo repo;

//        public UserStoryController(IUserStoriesRepo repo)
//        {
//            this.repo = repo;
//        }
//        [HttpGet("GetUsers")]
//        public IActionResult Get()
//        {
//            return Ok( repo.GetAll());
//        }

//        //[HttpPut("updateUser")]
//        //public Task<List<Register>> Update (int id , Register model)
//        //{

//        //    return repo.UpdateUser(id, model);

//        //}

//    }
//}

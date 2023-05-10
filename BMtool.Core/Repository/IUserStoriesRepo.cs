//using BMtool.Application.Models;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMtool.Application.Interfaces
{
    public interface IUserStoriesRepo
    {
      List<RegisterModel> GetAll();
        List<RegisterModel> Get(int id);
       List<RegisterModel> RegisterAsync(RegisterModel model);

        Task<List<RegisterModel>> UpdateUser(int id , RegisterModel model);
       
    }
}

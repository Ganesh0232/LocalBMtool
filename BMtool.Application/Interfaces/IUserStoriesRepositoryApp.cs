using BMtool.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMtool.Application.Interfaces
{
    public interface IUserStoriesRepositoryApp
    {
        Task<RegisterModel> GetAll();
        Task<RegisterModel> Get(int id);
        Task<RegisterModel> RegisterAsync(RegisterModel model);

        Task<List<RegisterModel>> UpdateAsync(int id , RegisterModel model);
       
    }
}

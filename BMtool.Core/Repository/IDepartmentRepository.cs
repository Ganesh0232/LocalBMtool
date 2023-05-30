using BMtool.Core.Entities;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;

namespace BMtool.Core.Repository
{
    public interface IDepartmentRepository
    {
        public List<Department> GetAllDepartmentListAsync();


        //G232

        List<UpdateDto> GetAllRegisteredUsers();
        List<UpdateDto> GetRegisteredUserById(int id);
        List<UpdateDto> RegisterAsync(UpdateDto model);

        Task UpdateRegisteredUserAsync(int id, UpdateDto model);

        Task UpdateRegisteredUserAsyncUsingStoredProc(int id, UpdateDto model);
        void Excelfile(string fileName);
        void Import();

    }

}

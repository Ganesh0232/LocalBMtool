using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMtool.Application.Interfaces;
using BMtool.Core.Entities;
using BMtool.Core.Repository;
using BMtool.Infrastructure.Data.Context;
using Dapper;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;

namespace BMtool.Infrastructure.Repository
{
    internal class UserStoryRepo : IUserStoriesRepo
    {
        public const string GetAllDepartmentsSql = "SELECT * FROM UserStoryTable";
        public BMtoolContext _context { get; }

        private readonly IDbConnection connection;

        public UserStoryRepo(BMtoolContext context)
        {
            this._context = context;
            this.connection = _context.CreateConnection();
        }


        public List<RegisterModel> Get(int id)
        {
            return connection.Query<RegisterModel>("Select * from UserStoryTable where Id =@Id", new {id=id}).ToList();
        }

        public List<RegisterModel> GetAll()
        {
            
            return  connection.Query<RegisterModel>("Select * from UserstoryTable").ToList();
        }

        public  List<RegisterModel> RegisterAsync(RegisterModel model)
        {
            var user =  connection.Query<RegisterModel>(" insert into UserStory19Table values ([@FirstName]  " +
                "    ,[@LastName]\r\n      ,[@Email]\r\n      ,[@Type]\r\n      ,[@Department]\r\n      ,[@Experience]\r\n  " +
                "    ,[@Role]\r\n      ,[@PhoneNumber])",model);
            var list = connection.Query<RegisterModel>(GetAllDepartmentsSql, model);

            return (list.ToList());
        }

        public async Task<List<RegisterModel>> UpdateUser(int id, RegisterModel model)
        {
            var user = connection.Query<RegisterModel>(" update  UserStory19Table set FirstName= ([@FirstName]  " +
                "  LastName  = [@LastName]     ,Email =[@Email]     ,Type =[@Type]     ,Department=[@Department]    ,Experience=[@Experience]  " +
                "    ,Role=[@Role]      ,Phonenumber=[@PhoneNumber] " +
                "where id=@id)", model);
            var U = await connection.QueryAsync<RegisterModel>(GetAllDepartmentsSql, model);

            return (U.ToList());
        }
    }
}

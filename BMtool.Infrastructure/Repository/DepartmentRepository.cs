﻿using BMtool.Core.Entities;
using BMtool.Core.Repository;
using BMtool.Infrastructure.Data.Context;
using Dapper;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace BMtool.Infrastructure.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly BMtoolContext _context;
        public const string GetAllDepartmentsSql = "SELECT Id,Name FROM Department";

        public DepartmentRepository(BMtoolContext bMtoolContext)
        {
            _context = bMtoolContext;
        }

        /// <summary>
        /// Retrieves a list of all departments from the database.
        /// </summary>
        /// <returns>A list of Department objects representing the departments.</returns>
        public List<Department> GetAllDepartmentListAsync()
        {
            var connection = _context.CreateConnection();
            return connection.Query<Department>(GetAllDepartmentsSql).ToList();
        }

        public List<UpdateDto> GetAllRegisteredUsers()
        {
            var connection = _context.CreateConnection();

            return connection.Query<UpdateDto>("Select * from Userstory19Table").ToList();
        }

        public List<UpdateDto> GetRegisteredUserById(int id)
        {
            var connection = _context.CreateConnection();
            var user = connection.Query<UpdateDto>("Select * from UserStory19Table where Id =@Id", new { id = id }).ToList();
            return user;
        }

        public List<UpdateDto> RegisterAsync(UpdateDto model)
        {
            var connection = _context.CreateConnection();

            //       var user = connection.Query<Register>(" insert into UserStory19Table(FirstName     ,LastName      ,Email      ,Type     ,Department     ,Experience      ,Role      ,PhoneNumber) values ([@FirstName]  " +
            //"    ,[@LastName]\r\n      ,[@Email]\r\n      ,[@Type]\r\n      ,[@Department]\r\n      ,[@Experience]\r\n  " +
            //"    ,[@Role]\r\n      ,[@PhoneNumber])", model);

            //var user = connection.Query<UpdateDto>(" insert into UserStory19Table (FirstName     ,LastName      ,PersonalEmail  ,OfficeEmail    ,EmployeeType     ,DepartmentId     ,Experience      ,Role      ,PhoneNumber)  values (@FirstName  ,@LastName   ,@Email      ,@Type     ,@Department     ,@Experience  ,@Role      ,@PhoneNumber)", model);

            var sql = @" insert into UserStory19Table
                        (FName ,LName ,PersonalEmail  ,OfficeEmail ,EmployeeType  
                        ,DepartmentId   ,Experience  ,Phone)  
                        values
                        (@fname, @lname,@PerEmail ,
                        @email,@type,@dept,@exp,
                       @number
                        )";
            var user = connection.Execute(sql, new
            {

                @fname = model.FName,
                @lname = model.LName,
                @PerEmail = model.PersonalEmail,
                @email = model.OfficeEmail,
                @type = model.EmployeeType,
                @dept = model.DepartmentId,
                @exp = model.Experience,
                // @role = model.Role,
                @number = model.Phone
            });
            var list = connection.Query<UpdateDto>(GetAllDepartmentsSql, model);

            return (list.ToList());
        }

        public async Task UpdateRegisteredUserAsync(int id, UpdateDto model)
        {
            //To check whether mailAddress is valid or not

            //    MailAddress mail = new MailAddress(model.Email);

            //  if (IsValidEmail(model.Email) && IsValidPhoneNumber(model.PhoneNumber) )
            {

                var connection = _context.CreateConnection();
                var sql = @"UPDATE [dbo].[UserStory19Table]
                     SET [FName] = @fname, [LName] = @lname,PersonalEmail = @PerEmail ,[OfficeEmail]=@email,[EmployeeType]=@type,[DepartmentId] =@dept,[Experience]=@exp,[Phone]=@number
                     WHERE Id = @id";

                var user = connection.Execute(sql, new
                {
                    @id = id,
                    @fname = model.FName,
                    @lname = model.LName,
                    @PerEmail = model.PersonalEmail,
                    @email = model.OfficeEmail,
                    @type = model.EmployeeType,
                    @dept = model.DepartmentId,
                    @exp = model.Experience,
                    //   @role = model.Role,
                    @number = model.Phone
                });

            }









            //try
            //{

            //var user = connection.Execute(" update  UserStory19Table set " +
            //    "FirstName= ([@FirstName]  " +
            //   " , LastName  = [@LastName]  " +
            //   "   ,Email =[@Email]   " +
            //   "  ,Type =[@Type]   " +
            //   "  ,Department=[@Department] " +
            //   "   ,Experience=[@Experience]  " +
            //   "    ,Role=[@Role]    " +
            //   "  ,Phonenumber=[@PhoneNumber] " +
            //   "where id=@id)",new { model.FirstName,model.LastName,model.Email,model.type,model.Department,model.Experience,model.Role,model.PhoneNumber,
            //   id} );

            //var user = connection.Execute(@" update  UserStory19Table set 
            //    FirstName= ([@FirstName]  
            //   , LastName  = [@LastName]  
            //      ,Email =[@Email]   
            //     ,Type =[@Type]  
            //     ,Department=[@Department] 
            //    ,Experience=[@Experience]  
            //     ,Role=[@Role]    
            //    ,Phonenumber=[@PhoneNumber] 
            //   where id=@id)", model);
            //  var U = await connection.QueryAsync<Register>(GetAllDepartmentsSql, model);

            //}
            //catch (Exception ex)
            //{
            //    return ex.Message;
            //}
        }

        static bool IsValidEmail(string email)
        {
            //  email = "ganesh@gmail.com";
            try
            {
                MailAddress mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        static bool IsValidPhoneNumber(string phoneNumber)
        {
            Regex regex = new Regex(@"^\+(?:[0-9]●?){6,14}[0-9]$");
            return regex.IsMatch(phoneNumber);
        }

        public async Task UpdateRegisteredUserAsyncUsingStoredProc(int id, UpdateDto model)
        {
            // Define the connection string
            var connection = _context.CreateConnection();

            // Define the stored procedure name
            string storedProcedure = "UpdateTable";

            // Create a new SqlConnection object
            using (connection)
            {
                // Execute the stored procedure using Dapper
                connection.Execute(storedProcedure, new { id, model.FName, model.LName,model.PersonalEmail, model.OfficeEmail, model.EmployeeType,model.DepartmentId,model.Experience,model.Phone }, commandType: CommandType.StoredProcedure);
            }
                                                                              //Fname, Lname, PersonalEmail, OfficeEmail, EmployeeType, DepartmentId, Experience, Phone
        }

        public void Excelfile(string fileName)
        {
            try
            {

                DataTable data = GetDataFromDatabase();
                //string filePath = "Path\\to\\your\\file.xlsx"; // Replace with your desired file path
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = Path.Combine(desktopPath, $"{fileName}.XLSX");


                ExportToExcel(data, filePath);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

       
        public DataTable GetDataFromDatabase()
        {

            string connectionString = "server=sailsinternal.database.windows.net;database=BMTool;Trusted_Connection=true"; // Replace with your actual connection string
            string query = "SELECT * FROM Users"; // Replace with your actual SQL query

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.ConnectionString = "Data Source=sailsinternal.database.windows.net;Initial Catalog=BMTool;User ID=sailsbm;Password=sail$Db123#";
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);

                    return dataTable;
                }
            }
        }

      
        public void ExportToExcel(DataTable dataTable, string filePath)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };
                sheets.Append(sheet);

                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                // Add header row
                Row headerRow = new Row();
                foreach (DataColumn column in dataTable.Columns)
                {
                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(column.ColumnName);
                    headerRow.AppendChild(cell);
                }
                sheetData.AppendChild(headerRow);

                // Add data rows
                foreach (DataRow row in dataTable.Rows)
                {
                    Row dataRow = new Row();
                    foreach (var item in row.ItemArray)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(item.ToString());
                        dataRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(dataRow);
                }

                workbookPart.Workbook.Save();
                document.Close();
            }
        }

    }
}

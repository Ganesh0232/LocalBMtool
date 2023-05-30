using System.Data;
using BMtool.Application.Interfaces;
using BMtool.Infrastructure.Data.Context;
using Dapper;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.Data.SqlClient;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DataTable = System.Data.DataTable;
using Workbook = DocumentFormat.OpenXml.Spreadsheet.Workbook;
using Worksheet = DocumentFormat.OpenXml.Spreadsheet.Worksheet;
using Sheets = DocumentFormat.OpenXml.Spreadsheet.Sheets;

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

        public void Excelfile()
        {
            try
            {

                DataTable data = GetDataFromDatabase();
                //string filePath = "Path\\to\\your\\file.xlsx"; // Replace with your desired file path
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = Path.Combine(desktopPath, "filename.xlsx");


                ExportToExcel(data, filePath);

            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public DataTable GetDataFromDatabase()
        {
            string connectionString = "server=(localdb)\\mssqllocaldb;database=BMtool_Demo;Trusted_Connection=true"; // Replace with your actual connection string
            string query = "SELECT * FROM users"; // Replace with your actual SQL query

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
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

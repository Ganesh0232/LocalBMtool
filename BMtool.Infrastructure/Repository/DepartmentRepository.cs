using BMtool.Core.Entities;
using BMtool.Core.Repository;
using BMtool.Infrastructure.Data.Context;
using Dapper;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using OfficeOpenXml;
using System.Data.SqlClient;
using BMtool.Application.Models;
using System.Text;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using ExcelDataReader;
using DocumentFormat.OpenXml.Office2010.Excel;

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
                connection.Execute(storedProcedure, new { id, model.FName, model.LName, model.PersonalEmail, model.OfficeEmail, model.EmployeeType, model.DepartmentId, model.Experience, model.Phone }, commandType: CommandType.StoredProcedure);
            }
            //Fname, Lname, PersonalEmail, OfficeEmail, EmployeeType, DepartmentId, Experience, Phone
        }

        public void Excelfile1(string fileName)
        {
            try
            {

                DataTable data = GetDataFromDatabase1();


                //string filePath = "Path\\to\\your\\file.xlsx"; // Replace with your desired file path
                //  string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                //string desktopPath = @"C:\Users\SAILS-DM292\Source\Repos\LocalBMtool\BMtool.Api\ExportedFile";
                //string filePath = Path.Combine(desktopPath, $"{fileName}_{DateTime.Now}.XLSX");


                //  string folderPath = @"C:\Users\SAILS-DM292\Source\Repos\LocalBMtool\BMtool.Api\ExportedFile";
                string folderPath = @"C:\\Ganesh232\\";

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                fileName = $"{fileName}_{timestamp}.csv";
                string filePath = Path.Combine(folderPath, fileName);



                ExportToExcel1(data, filePath);

            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public DataTable GetDataFromDatabase1()
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


        public void ExportToExcel1(DataTable dataTable, string filePath)
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



        //Today export

        public DataTable GetDataFromDatabase(string query)
        {
            string connectionString = "server=sailsinternal.database.windows.net;database=BMTool;Trusted_Connection=true"; // Replace with your actual connection string

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


        public void ExportToExcel12(DataSet dataSet, string filePath)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                foreach (DataTable dataTable in dataSet.Tables)
                {
                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = (uint)(sheets.Count() + 1), Name = dataTable.TableName };
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
                }

                workbookPart.Workbook.Save();
                document.Close();
            }
        }

        public void ExportToExcel(DataSet dataSet, string filePath)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                foreach (DataTable dataTable in dataSet.Tables)
                {
                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    string sheetName = dataTable.TableName;

                    // Generate a unique sheet ID
                    uint sheetId = 1;
                    if (sheets.Elements<Sheet>().Any())
                    {
                        sheetId = sheets.Elements<Sheet>().Max(s => s.SheetId.Value) + 1;
                    }

                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = sheetId, Name = sheetName };
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
                }

                workbookPart.Workbook.Save();
                document.Close();
            }
        }

        public void Excelfile12(string fileName)
        {
            try
            {
                List<string> queries = new List<string>
        {
            "SELECT * FROM Users",
            "SELECT * FROM Department",
            "SELECT * FROM Designation",
            "SELECT * FROM EmployeeSkillSet",
            "SELECT * FROM EventAttendees",
            "SELECT * FROM Events",
             "SELECT * FROM Learning",
            "SELECT * FROM Notes",
            "SELECT * FROM PasswordResetToken",
            "SELECT * FROM Projects",
            "SELECT * FROM ProjectTechEntries",
            "SELECT * FROM Roles",
            "SELECT * FROM Tasks",
            "SELECT * FROM Technologies",
            "SELECT * FROM Upskilling",
            "SELECT * FROM UserProjectEntries",
            "SELECT * FROM UserRoles"

        };

                DataSet dataSet = new DataSet();
                foreach (string query in queries)
                {
                    DataTable table = GetDataFromDatabase(query);
                    dataSet.Tables.Add(table);
                }

                string folderPath = @"C:\\Ganesh232\\";
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                fileName = $"{fileName}_{timestamp}.xlsx";
                string filePath = Path.Combine(folderPath, fileName);

                ExportToExcel(dataSet, filePath);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Excelfile(string fileName)
        {
            try
            {
                Dictionary<string, string> tableQueries = new Dictionary<string, string>
        {
            {"Users", "SELECT * FROM Users"},
            {"Department", "SELECT * FROM Department"},
            {"Designation", "SELECT * FROM Designation"},
            {"EmployeeSkillSet", "SELECT * FROM EmployeeSkillSet"},
            {"EventAttendees", "SELECT * FROM EventAttendees"},
            {"Events", "SELECT * FROM Events"},
            {"Learning", "SELECT * FROM Learning"},
            {"Notes", "SELECT * FROM Notes"},
            {"PasswordResetToken", "SELECT * FROM PasswordResetToken"},
            {"Projects", "SELECT * FROM Projects"},
            {"ProjectTechEntries", "SELECT * FROM ProjectTechEntries"},
            {"Roles", "SELECT * FROM Roles"},
            {"Tasks", "SELECT * FROM Tasks"},
            {"Technologies", "SELECT * FROM Technologies"},
            {"Upskilling", "SELECT * FROM Upskilling"},
            {"UserProjectEntries", "SELECT * FROM UserProjectEntries"},
            {"UserRoles", "SELECT * FROM UserRoles"}
        };

                DataSet dataSet = new DataSet();
                foreach (var tableQuery in tableQueries)
                {
                    string tableName = tableQuery.Key;
                    string query = tableQuery.Value;

                    DataTable table = GetDataFromDatabase(query);
                    table.TableName = tableName;
                    dataSet.Tables.Add(table);
                }

                string folderPath = @"C:\\Ganesh232\\";
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                fileName = $"{fileName}_{timestamp}.xlsx";
                string filePath = Path.Combine(folderPath, fileName);

                ExportToExcel(dataSet, filePath);
            }
            catch (Exception ex)
            {
                throw;
            }
        }




        //public void ImportExcelDataToSQL(string excelFilePath, string sqlConnectionString)
        //{
        //    Excel.Application excelApp = new Excel.Application();
        //    Excel.Workbook workbook = excelApp.Workbooks.Open(excelFilePath);
        //    Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the data is on the first sheet

        //    // Get the range of used cells in the worksheet
        //    Excel.Range usedRange = worksheet.UsedRange;

        //    // Get the number of rows and columns in the used range
        //    int rowCount = usedRange.Rows.Count;
        //    int columnCount = usedRange.Columns.Count;

        //    // Open a connection to the SQL Server database
        //    using (SqlConnection connection = new SqlConnection(sqlConnectionString))
        //    {
        //        connection.Open();

        //        // Iterate over the rows and columns in the Excel file
        //        for (int row = 1; row <= rowCount; row++)
        //        {
        //            // Assuming the data starts from the first row

        //            // Create a parameterized SQL INSERT statement
        //            string insertQuery = "INSERT INTO TableName (Column1, Column2, Column3) VALUES (@Column1, @Column2, @Column3)";

        //            // Create a SQL command object
        //            using (SqlCommand command = new SqlCommand(insertQuery, connection))
        //            {
        //                // Set the parameter values from the Excel file
        //                command.Parameters.AddWithValue("@Column1", (worksheet.Cells[row, 1] as Excel.Range).Value2);
        //                command.Parameters.AddWithValue("@Column2", (worksheet.Cells[row, 2] as Excel.Range).Value2);
        //                command.Parameters.AddWithValue("@Column3", (worksheet.Cells[row, 3] as Excel.Range).Value2);

        //                // Execute the SQL command
        //                command.ExecuteNonQuery();
        //            }
        //        }

        //        connection.Close();
        //    }

        //    // Clean up Excel objects
        //    workbook.Close();
        //    excelApp.Quit();
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        //    worksheet = null;
        //    workbook = null;
        //    excelApp = null;
        //    GC.Collect();
        //}

        //public void Import(string excelFilePath)
        //{
        //    try
        //    {
        //        excelFilePath = @"C:\Users\SAILS-DM292\Source\Repos\LocalBMtool\BMtool.Api\ExportedFile";
        //        //string excelFilePath = @"C:\Users\SAILS-DM292\Source\Repos\LocalBMtool\BMtool.Api\bin\Debug\net7.0";



        //        string sqlConnectionString = @"server=(localdb)\\mssqllocaldb;database=BMtool_Demo;Trusted_Connection=true";

        //        ImportExcelDataToSQL(excelFilePath, sqlConnectionString);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }


        //}

        //public static void Main()
        //{

        //  OpenFileDialog openFileDialog = new OpenFileDialog();

        //    // Set the initial directory (optional)
        //    openFileDialog.InitialDirectory = @"C:\";

        //    // Set the title of the dialog (optional)
        //    openFileDialog.Title = "Select a File";

        //    // Set the filter for the types of files to be displayed
        //    openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

        //    // Set whether multiple files can be selected (optional)
        //    openFileDialog.Multiselect = false;

        //    // Show the dialog and check if the user clicked the "OK" button
        //    if (openFileDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        // Get the selected file path
        //        string selectedFilePath = openFileDialog.FileName;

        //        // Do something with the selected file
        //        Console.WriteLine("Selected File: " + selectedFilePath);
        //    }
        //}





        public void ImportOledb()
        {
            //string excelFilePath = "C:\\Ganesh232";
            ExcelPackage excelFilePath = new ExcelPackage(new FileInfo("C:\\Ganesh232\\CSv_20230525_115352.csv"));
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={excelFilePath};Extended Properties=\"Excel 12.0;HDR=YES;\"";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                // Get the list of sheets in the Excel file
                DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                if (schemaTable != null)
                {
                    foreach (DataRow row in schemaTable.Rows)
                    {
                        string sheetName = row["TABLE_NAME"].ToString();

                        // Read the data from each sheet
                        string query = $"SELECT * FROM [{sheetName}]";
                        OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, connection);
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        // Store the data in the SQL database
                        //  string sqlConnectionString = "server=(localdb)\\mssqllocaldb;database=BMtool_Demo;Trusted_Connection=true";
                        string sqlConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=BMtool_Demo;Integrated Security=True;";

                        using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                        {
                            sqlConnection.Open();

                            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection))
                            {
                                bulkCopy.DestinationTableName = "users19";
                                bulkCopy.WriteToServer(dataTable);
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Data imported successfully!");
        }



        public void Importo()
        {
            string excelFilePath = "C:\\Ganesh232\\CSv_20230525_115352.csv";

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0]; // Assuming the data is on the first worksheet

                int rows = worksheet.Dimension.Rows;
                int columns = worksheet.Dimension.Columns;

                DataTable dataTable = new DataTable();

                // Read column names from the first row and add them as columns to the DataTable
                for (int col = 1; col <= columns; col++)
                {
                    dataTable.Columns.Add(worksheet.Cells[1, col].Value.ToString());
                }

                // Read data starting from the second row and populate the DataTable
                for (int row = 2; row <= rows; row++)
                {
                    DataRow dataRow = dataTable.Rows.Add();

                    for (int col = 1; col <= columns; col++)
                    {

                        dataRow[col - 1] = worksheet.Cells[row, col].Value;
                    }
                }

                // Store the data in the SQL database
                string sqlConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=BMtool_Demo;Integrated Security=True;";

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    sqlConnection.Open();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection))
                    {
                        bulkCopy.DestinationTableName = "users19";
                        bulkCopy.WriteToServer(dataTable);
                    }
                }
            }

            Console.WriteLine("Data imported successfully!");
        }

        //Use this
        public void Import222()
        {
            string excelFilePath = "C:\\Ganesh232\\Import.xlsx";

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0]; // Assuming the data is on the first worksheet

                int rows = worksheet.Dimension.Rows;
                int columns = worksheet.Dimension.Columns;

                DataTable dataTable = new DataTable();

                // Read column names from the first row and add them as columns to the DataTable
                for (int col = 1; col <= columns; col++)
                {
                    dataTable.Columns.Add(worksheet.Cells[1, col].Value.ToString());
                }


                // Read data starting from the second row and populate the DataTable
                for (int row = 2; row <= rows; row++)
                {
                    DataRow dataRow = dataTable.Rows.Add();

                    for (int col = 2; col <= columns; col++)
                    {
                        string cellValue = worksheet.Cells[row, col].Value?.ToString();

                        if (col == 19) // Assuming column index 19 is for IsFirstLogin
                        {
                            // Convert the string value to a bool
                            bool isFirstLogin;
                            if (bool.TryParse(cellValue, out isFirstLogin))
                            {
                                dataRow[col - 1] = isFirstLogin;
                            }
                            else
                            {
                                // Handle the conversion failure, you can assign a default value or handle it as per your requirement
                                dataRow[col - 1] = DBNull.Value; // or any other appropriate value
                            }
                        }

                        if (col == 5) // Assuming column index 5 is for DOB
                        {
                            // Check if the value is empty or null
                            if (!string.IsNullOrEmpty(cellValue))
                            {
                                // Convert the value to DateTime using a specific format
                                DateTime dob;
                                if (DateTime.TryParseExact(cellValue, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out dob))
                                {
                                    dataRow[col - 1] = dob;
                                }
                                else
                                {
                                    // Handle the conversion failure, you can assign a default value or handle it as per your requirement
                                    dataRow[col - 1] = DBNull.Value; // or any other appropriate value
                                }
                            }
                            else
                            {
                                // Handle empty or null value, you can assign a default value or handle it as per your requirement
                                dataRow[col - 1] = DBNull.Value; // or any other appropriate value
                            }
                        }
                        else
                        {
                            dataRow[col - 1] = cellValue;
                        }

                        if (col == 10) // Assuming column index 10 is for EmployeeType
                        {
                            // Check if the value is empty or null
                            if (!string.IsNullOrEmpty(cellValue))
                            {
                                // Convert the non-empty value to a Boolean (bit)
                                bool employeeType;
                                if (bool.TryParse(cellValue, out employeeType))
                                {
                                    dataRow[col - 1] = employeeType;
                                }
                                else
                                {
                                    Console.WriteLine($"Failed to convert value '{cellValue}' to Boolean (bit) in row {row}, column {col}.");
                                    // Handle the conversion failure, you can assign a default value or handle it as per your requirement
                                    dataRow[col - 1] = DBNull.Value; // or any other appropriate value
                                }
                            }
                            else
                            {
                                // Handle empty or null value, you can assign a default value or handle it as per your requirement
                                dataRow[col - 1] = DBNull.Value; // or any other appropriate value
                            }
                        }
                        else
                        {
                            dataRow[col - 1] = cellValue;
                        }
                    }
                }





                // Store the data in the SQL database
                string sqlConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=BMtool_Demo;Integrated Security=True;";

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    sqlConnection.Open();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection))
                    {
                        bulkCopy.DestinationTableName = "users";
                        bulkCopy.WriteToServer(dataTable);
                    }
                }
            }

            Console.WriteLine("Data imported successfully!");
        }

        public void Import269()
        {
            string excelFilePath = "C:\\Ganesh232\\Import.xlsx";

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0]; // Assuming the data is on the first worksheet

                int rows = worksheet.Dimension.Rows;
                int columns = worksheet.Dimension.Columns;

                DataTable dataTable = new DataTable();

                // Read column names from the first row and add them as columns to the DataTable
                for (int col = 1; col <= columns; col++)
                {
                    dataTable.Columns.Add(worksheet.Cells[1, col].Value.ToString());
                }

                // Read data starting from the second row and populate the DataTable
                for (int row = 2; row <= rows; row++)
                {
                    DataRow dataRow = dataTable.Rows.Add();

                    for (int col = 2; col <= columns; col++)
                    {
                        string cellValue = worksheet.Cells[row, col].Value?.ToString();

                        if (col == 19) // Assuming column index 19 is for IsFirstLogin
                        {
                            // Convert the string value to a bool
                            bool isFirstLogin;
                            if (bool.TryParse(cellValue, out isFirstLogin))
                            {
                                dataRow[col - 1] = isFirstLogin;
                            }
                            else
                            {
                                dataRow[col - 1] = DBNull.Value; // or any other appropriate value
                            }
                        }

                        if (col == 5) // Assuming column index 5 is for DOB
                        {
                            // Handle empty or null value
                            if (!string.IsNullOrEmpty(cellValue))
                            {
                                dataRow[col - 1] = DateTime.Parse(cellValue);
                            }
                            else
                            {
                                dataRow[col - 1] = DBNull.Value; // or any other appropriate value
                            }
                        }

                        if (col == 10) // Assuming column index 10 is for EmployeeType
                        {
                            // Check if the value is empty or null
                            if (!string.IsNullOrEmpty(cellValue))
                            {
                                // Convert the non-empty value to a Boolean (bit)
                                bool employeeType;
                                if (bool.TryParse(cellValue, out employeeType))
                                {
                                    dataRow[col - 1] = employeeType;
                                }
                                else
                                {
                                    dataRow[col - 1] = DBNull.Value; // or any other appropriate value
                                }
                            }
                            else
                            {
                                dataRow[col - 1] = DBNull.Value; // or any other appropriate value
                            }
                        }
                        else
                        {
                            dataRow[col - 1] = cellValue;
                        }
                    }
                }

                // Store the data in the SQL database
                string sqlConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=BMtool_Demo;Integrated Security=True;";

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    sqlConnection.Open();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection))
                    {
                        bulkCopy.DestinationTableName = "users";
                        bulkCopy.WriteToServer(dataTable);
                    }
                }
            }

            Console.WriteLine("Data imported successfully!");
        }


        public void Import238()
        {
            string excelFilePath = "C:\\Ganesh232\\CSv_20230525_115352.csv";

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0]; // Assuming the data is on the first worksheet

                int rows = worksheet.Dimension.Rows;
                int columns = worksheet.Dimension.Columns;

                DataTable dataTable = new DataTable();

                // Read column names from the first row and add them as columns to the DataTable
                for (int col = 1; col <= columns; col++)
                {
                    dataTable.Columns.Add(worksheet.Cells[1, col].Value.ToString());
                }

                // Read data starting from the second row and populate the DataTable


                //for (int row = 2; row <= rows; row++)
                //{
                //    DataRow dataRow = dataTable.Rows.Add();

                //    for (int col = 1; col <= columns; col++)
                //    {
                //        var cellValue = worksheet.Cells[row, col].Value;

                //        if (col == 5 && cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
                //        {
                //            if (DateTime.TryParse(cellValue.ToString(), out DateTime dateValue))
                //            {
                //                dataRow[col - 1] = dateValue;
                //            }
                //            else
                //            {
                //                // Handle invalid date format or empty DOB value
                //                // You can assign a default value or handle the error as per your requirement
                //                dataRow[col - 1] = DBNull.Value; // Set to DBNull or provide a default value
                //            }
                //        }
                //        else
                //        {
                //            dataRow[col - 1] = cellValue;
                //        }
                //    }
                //}

                for (int row = 2; row <= rows; row++)
                {
                    DataRow dataRow = dataTable.Rows.Add();


                    for (int col = 1; col <= columns; col++)
                    {
                        var cellValue = worksheet.Cells[row, col].Value;

                        // Check if the cell value is null or contains only spaces
                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            dataRow[col - 1] = null;  // Replace space with null or the desired value
                        }
                        else
                        {
                            dataRow[col - 1] = cellValue;
                        }
                    }
                }


                // Store the data in the SQL database
                string sqlConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=BMtool_Demo;Integrated Security=True;";

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    sqlConnection.Open();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection))
                    {
                        bulkCopy.DestinationTableName = "users19";
                        bulkCopy.WriteToServer(dataTable);
                    }
                }
            }

            Console.WriteLine("Data imported successfully!");
        }


        public void Import1()
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo("C:\\Ganesh232\\CSv_20230525_115352.csv")))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                string connectionString = "server=(localdb)\\mssqllocaldb;database=BMtool_Demo;Trusted_Connection=true";

                int worksheetCount = package.Workbook.Worksheets.Count;

                if (worksheetCount > 0)
                {
                    int worksheetIndex = 0; // Use the appropriate index for the desired worksheet

                    if (worksheetIndex >= 0 && worksheetIndex < worksheetCount)
                    {



                        ExcelWorksheet worksheet = package.Workbook.Worksheets[worksheetIndex]; // Assuming the data is in the first worksheet

                        int rows = worksheet.Dimension.Rows;
                        int columns = worksheet.Dimension.Columns;

                        // Iterate through each row and column to retrieve the data
                        for (int row = 1; row <= rows; row++)
                        {
                            for (int col = 1; col <= columns; col++)
                            {
                                string cellValue = worksheet.Cells[row, col].Value?.ToString();
                                // Process the cell value and insert it into the database
                                // You can use the SqlConnection and SqlCommand classes to insert data into the database

                                string query = @"INSERT INTO Users19  ([EmployeeNumber] ,[FName] ,[LName]   ,[Gender] ,[DOB]
                                                            ,[Phone] ,[OfficeEmail] ,[Mobile]  ,[PersonalEmail]  ,[EmployeeType]   ,[Experience]  ,[DesignationId]
                                                             ,[IsInProject],[IsUpskilling] ,[IsWorkingOnInternalTool] ,[ProjectId],[NotesId],[Password]
                                                             ,[IsFirstLogin],[DeptId],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn],[JoinedOn]
                                                             ,[Location],[IsActive]) 
                                                    VALUES (@EmployeeNumber, @FName, @LName, @Gender, @DOB, @Phone, @OfficeEmail, @Mobile, @PersonalEmail,
                                                            @EmployeeType, @Experience, @DesignationId, @IsInProject, @IsUpskilling, @IsWorkingOnInternalTool, 
                                                            @ProjectId, @NotesId, @Password, @IsFirstLogin, @DeptId, @CreatedBy, @CreatedOn, @ModifiedBy, @ModifiedOn,
                                                            @JoinedOn, @Location, @IsActive);";

                                //  SqlCommand command = new SqlCommand(query, connection);
                                //    command.Parameters.AddWithValue("@value", cellValue);

                                // Create a list of data sets to import
                                using (SqlConnection connection = new SqlConnection(connectionString))
                                {
                                    connection.Open();

                                    List<ImportModelclass> dataSets = new List<ImportModelclass>
                                    {
                                        new ImportModelclass { EmployeeNumber = "405", FName = "John",
                                            LName = "Doe", Gender = "M", DOB = new DateTime(1990, 1, 1), Phone = "1234567890",
                                            OfficeEmail = "john.doe@company.com", Mobile = "9876543210", PersonalEmail = "johndoe@gmail.com",
                                            EmployeeType = 1, Experience = "32", DesignationId = 1, IsInProject = 1, IsUpskilling = 0,
                                            IsWorkingOnInternalTool = 0, ProjectId = 1, NotesId = 1, Password = "abcd123", IsFirstLogin = '1',
                                            DeptId = 1, CreatedBy = 18, CreatedOn = DateTime.Now, ModifiedBy = 18, ModifiedOn = DateTime.Now,
                                            JoinedOn = new DateTime(2022, 1, 1), Location = "New York", IsActive = 1},


                                      // Add more data sets as needed
                                    };

                                    foreach (ImportModelclass data in dataSets)
                                    {
                                        SqlCommand command = new SqlCommand(query, connection);

                                        // Assign parameter values for each data set
                                        command.Parameters.AddWithValue("@EmployeeNumber", data.EmployeeNumber);
                                        command.Parameters.AddWithValue("@FName", data.FName);
                                        command.Parameters.AddWithValue("@LName", data.LName);
                                        command.Parameters.AddWithValue("@Gender", data.Gender);
                                        command.Parameters.AddWithValue("@DOB", data.DOB);
                                        command.Parameters.AddWithValue("@Phone", data.Phone);
                                        command.Parameters.AddWithValue("@OfficeEmail", data.OfficeEmail);
                                        command.Parameters.AddWithValue("@Mobile", data.Mobile);
                                        command.Parameters.AddWithValue("@PersonalEmail", data.PersonalEmail);
                                        command.Parameters.AddWithValue("@EmployeeType", data.EmployeeType);
                                        command.Parameters.AddWithValue("@Experience", data.Experience);
                                        command.Parameters.AddWithValue("@DesignationId", data.DesignationId);
                                        command.Parameters.AddWithValue("@IsInProject", data.IsInProject);
                                        command.Parameters.AddWithValue("@IsUpskilling", data.IsUpskilling);
                                        command.Parameters.AddWithValue("@IsWorkingOnInternalTool", data.IsWorkingOnInternalTool);
                                        command.Parameters.AddWithValue("@ProjectId", data.ProjectId);
                                        command.Parameters.AddWithValue("@NotesId", data.NotesId);
                                        command.Parameters.AddWithValue("@Password", data.Password);
                                        command.Parameters.AddWithValue("@IsFirstLogin", data.IsFirstLogin);
                                        command.Parameters.AddWithValue("@DeptId", data.DeptId);
                                        command.Parameters.AddWithValue("@CreatedBy", data.CreatedBy);
                                        command.Parameters.AddWithValue("@CreatedOn", data.CreatedOn);
                                        command.Parameters.AddWithValue("@ModifiedBy", data.ModifiedBy);
                                        command.Parameters.AddWithValue("@ModifiedOn", data.ModifiedOn);
                                        command.Parameters.AddWithValue("@JoinedOn", data.JoinedOn);
                                        command.Parameters.AddWithValue("@Location", data.Location);
                                        command.Parameters.AddWithValue("@IsActive", data.IsActive);

                                        int rowsAffected = command.ExecuteNonQuery();
                                        Console.WriteLine($"{rowsAffected} row(s) inserted for data set.");
                                    }


                                    connection.Close();
                                    //command.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid worksheet index");
                    }

                }
            }
        }


        //Today
        public void Import011()
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo("C:\\Ganesh232\\CSv_20230525_115352.csv")))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                string connectionString = "server=(localdb)\\mssqllocaldb;database=BMtool_Demo;Trusted_Connection=true";

                int worksheetCount = package.Workbook.Worksheets.Count;

                if (worksheetCount > 0)
                {
                    int worksheetIndex = 0; // Use the appropriate index for the desired worksheet

                    if (worksheetIndex >= 0 && worksheetIndex < worksheetCount)
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[worksheetIndex]; // Assuming the data is in the first worksheet

                        int rows = worksheet.Dimension.Rows;
                        int columns = worksheet.Dimension.Columns;
                        DataTable dataTable = new DataTable();

                        // Retrieve the column names from the first row
                        var columnNames = new List<string>();
                        for (int col = 1; col <= columns; col++)
                        {
                            string columnName = worksheet.Cells[1, col].Value?.ToString();
                            columnNames.Add(columnName);
                        }

                        // Iterate through each row (excluding the first row) to retrieve the data
                        for (int row = 2; row <= rows; row++)
                        {
                            DataRow dataRow = dataTable.Rows.Add();

                            for (int col = 1; col <= columns; col++)
                            {
                                string cellValue = worksheet.Cells[row, col].Value?.ToString();

                                // Get the column name for the current column
                                string columnName = columnNames[col - 1];

                                // Process the cell value and insert it into the database
                                // You can use the SqlConnection and SqlCommand classes to insert data into the database

                                string query = $"INSERT INTO Users19 ({columnName}) VALUES (@value);";

                                using (SqlConnection connection = new SqlConnection(connectionString))
                                {
                                    connection.Open();





                                    SqlCommand command = new SqlCommand(query, connection);
                                    command.Parameters.AddWithValue("@value", cellValue);

                                    int rowsAffected = command.ExecuteNonQuery();
                                    Console.WriteLine($"{rowsAffected} row(s) inserted.");
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid worksheet index");
                    }
                }
            }
        }

        public void Import12()
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo("C:\\Ganesh232\\CSv_20230525_115352.csv")))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                string connectionString = "server=(localdb)\\mssqllocaldb;database=BMtool_Demo;Trusted_Connection=true";

                int worksheetCount = package.Workbook.Worksheets.Count;

                if (worksheetCount > 0)
                {
                    int worksheetIndex = 0; // Use the appropriate index for the desired worksheet

                    if (worksheetIndex >= 0 && worksheetIndex < worksheetCount)
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[worksheetIndex]; // Assuming the data is in the first worksheet

                        int rows = worksheet.Dimension.Rows;
                        int columns = worksheet.Dimension.Columns;

                        // Iterate through each row and column to retrieve the data
                        for (int row = 1; row <= rows; row++)
                        {
                            // Create a new list to store the parameter values
                            List<SqlParameter> parameters = new List<SqlParameter>();

                            for (int col = 1; col <= columns; col++)
                            {
                                string columnName = worksheet.Cells[1, col].Value?.ToString();
                                string cellValue = worksheet.Cells[row, col].Value?.ToString();

                                // Exclude the identity column from the insert statement
                                if (columnName != "ID")
                                {
                                    SqlParameter parameter = new SqlParameter($"@{columnName}", cellValue);
                                    parameters.Add(parameter);
                                }
                            }

                            // Create the SQL INSERT statement
                            string query = @"INSERT INTO Users19  ([EmployeeNumber] ,[FName] ,[LName]   ,[Gender] ,[DOB]
                                                            ,[Phone] ,[OfficeEmail] ,[Mobile]  ,[PersonalEmail]  ,[EmployeeType]   ,[Experience]  ,[DesignationId]
                                                             ,[IsInProject],[IsUpskilling] ,[IsWorkingOnInternalTool] ,[ProjectId],[NotesId],[Password]
                                                             ,[IsFirstLogin],[DeptId],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn],[JoinedOn]
                                                             ,[Location],[IsActive]) 
                                                    VALUES (@EmployeeNumber, @FName, @LName, @Gender, @DOB, @Phone, @OfficeEmail, @Mobile, @PersonalEmail,
                                                            @EmployeeType, @Experience, @DesignationId, @IsInProject, @IsUpskilling, @IsWorkingOnInternalTool, 
                                                            @ProjectId, @NotesId, @Password, @IsFirstLogin, @DeptId, @CreatedBy, @CreatedOn, @ModifiedBy, @ModifiedOn,
                                                            @JoinedOn, @Location, @IsActive);";

                            // Execute the SQL INSERT statement with parameters
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                connection.Open();

                                SqlCommand command = new SqlCommand(query, connection);
                                command.Parameters.AddRange(parameters.ToArray());
                                command.ExecuteNonQuery();

                                connection.Close();
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid worksheet index");
                    }
                }
            }
        }


        public void Import05()
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo("C:\\Ganesh232\\CSv_20230525_115352.csv")))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                string connectionString = "server=(localdb)\\mssqllocaldb;database=BMtool_Demo;Trusted_Connection=true";

                int worksheetCount = package.Workbook.Worksheets.Count;

                if (worksheetCount > 0)
                {
                    int worksheetIndex = 0; // Use the appropriate index for the desired worksheet

                    if (worksheetIndex >= 0 && worksheetIndex < worksheetCount)
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[worksheetIndex]; // Assuming the data is in the first worksheet

                        int rows = worksheet.Dimension.Rows;
                        int columns = worksheet.Dimension.Columns;

                        // Iterate through each row and column to retrieve the data
                        for (int row = 1; row <= rows; row++)
                        {
                            // Create a new list to store the parameter values
                            List<SqlParameter> parameters = new List<SqlParameter>();

                            for (int col = 1; col <= columns; col++)
                            {
                                string columnName = worksheet.Cells[1, col].Value?.ToString();
                                string cellValue = worksheet.Cells[row, col].Value?.ToString();

                                // Exclude the identity column from the insert statement
                                if (columnName != "ID")
                                {

                                    SqlParameter parameter = new SqlParameter($"@{columnName}", cellValue);
                                    parameters.Add(parameter);

                                }
                            }

                            // Create the SQL INSERT statement
                            string query = @"INSERT INTO Users19 ([EmployeeNumber], [FName], [LName], [Gender], [DOB], [Phone], [OfficeEmail], [Mobile],
                                                        [PersonalEmail], [EmployeeType], [Experience], [DesignationId], [IsInProject], [IsUpskilling],
                                                        [IsWorkingOnInternalTool], [ProjectId], [NotesId], [Password], [IsFirstLogin], [DeptId], [CreatedBy],
                                                        [CreatedOn], [ModifiedBy], [ModifiedOn], [JoinedOn], [Location], [IsActive]) 
                                    VALUES (@EmployeeNumber, @FName, @LName, @Gender, @DOB, @Phone, @OfficeEmail, @Mobile, @PersonalEmail,
                                            @EmployeeType, @Experience, @DesignationId, @IsInProject, @IsUpskilling, @IsWorkingOnInternalTool, 
                                            @ProjectId, @NotesId, @Password, @IsFirstLogin, @DeptId, @CreatedBy, @CreatedOn, @ModifiedBy, @ModifiedOn,
                                            @JoinedOn, @Location, @IsActive);";

                            // Execute the SQL INSERT statement with parameters
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                connection.Open();

                                SqlCommand command = new SqlCommand(query, connection);
                                command.Parameters.AddRange(parameters.ToArray());
                                command.ExecuteNonQuery();

                                connection.Close();
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid worksheet index");
                    }
                }
            }
        }


        public void Import06()
        {
            string excelFilePath = "C:\\Ganesh232\\CSv_20230525_115352.csv";

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0]; // Assuming the data is on the first worksheet

                int rows = worksheet.Dimension.Rows;
                int columns = worksheet.Dimension.Columns;

                DataTable dataTable = new DataTable();

                // Read column names from the first row and add them as columns to the DataTable
                for (int col = 1; col <= columns; col++)
                {
                    dataTable.Columns.Add(worksheet.Cells[1, col].Value.ToString());
                }



                // Read data starting from the second row and populate the DataTable
                for (int row = 2; row <= rows; row++)
                {
                    DataRow dataRow = dataTable.Rows.Add();

                    for (int col = 1; col <= columns; col++)
                    {
                        string cellValue = worksheet.Cells[row, col].Value?.ToString();
                        DateTime dateValue;

                        if (col == 5 && !DateTime.TryParseExact(cellValue, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                        {
                            Console.WriteLine($"Failed to convert value '{cellValue}' to DateTime in row {row}, column {col}.");
                        }

                        dataRow[col - 1] = cellValue;
                    }
                }



                // Store the data in the SQL database
                string sqlConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=BMtool_Demo;Integrated Security=True;";

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    sqlConnection.Open();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection))
                    {
                        bulkCopy.DestinationTableName = "users19";
                        bulkCopy.WriteToServer(dataTable);
                    }
                }
            }

            Console.WriteLine("Data imported successfully!");
        }


        //Bhavya
       public void Import()
        {

          

            // Establish the connection string to the database
            var connectionString = "server=(localdb)\\mssqllocaldb;database=BMtool_Demo;Trusted_Connection=true";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Create a connection to the database
            using (var connection = new SqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Load the Excel file
                using (var package = new ExcelPackage(new FileInfo("C:\\Ganesh232\\Import.xlsx")))
                {
                    // Assuming the data is in the first worksheet of the Excel file
                    var worksheet = package.Workbook.Worksheets[0];

                    // Create a DataTable to hold the data
                    var table = new DataTable();

                    // Define the columns in the DataTable
                    table.Columns.Add("Id", typeof(int)); 
                    table.Columns.Add("EmployeeNumber", typeof(string));
                    table.Columns.Add("FName", typeof(string));
                    table.Columns.Add("LName", typeof(string));                    
                    table.Columns.Add("Gender", typeof(string));
                    table.Columns.Add("Phone", typeof(string));
                    table.Columns.Add("DOB", typeof(DateTime));                    
                    table.Columns.Add("OfficeEmail", typeof(string));
                    table.Columns.Add("Mobile", typeof(string));
                    table.Columns.Add("PersonalEmail", typeof(string));
                    table.Columns.Add("EmployeeType", typeof(bool));
                    table.Columns.Add("Experience", typeof(byte));
                    table.Columns.Add("DesignationId", typeof(byte));
                    table.Columns.Add("IsInProject", typeof(bool));
                    table.Columns.Add("IsUpskilling", typeof(bool));
                    table.Columns.Add("IsWorkingOnInternalTool", typeof(bool));
                    table.Columns.Add("ProjectId", typeof(byte));
                    table.Columns.Add("NotesId", typeof(int));
                    table.Columns.Add("IsFirstLogin", typeof(bool));
                    table.Columns.Add("DeptId", typeof(byte));
                    table.Columns.Add("CreatedBy", typeof(int));
                    table.Columns.Add("CreatedOn", typeof(DateTime));
                    table.Columns.Add("ModifiedBy", typeof(int));
                    table.Columns.Add("ModifiedOn", typeof(DateTime));
                    table.Columns.Add("JoinedOn", typeof(DateTime));
                    table.Columns.Add("Location", typeof(string));
                    table.Columns.Add("IsActive", typeof(bool));
                 //   table.Columns.Add("Password", typeof(byte[]));

                    // Iterate over the rows in the worksheet
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++) // Assuming the first row is a header
                    {
                        // Read the data from the Excel file
                        var id = worksheet.Cells[row, 1].GetValue<int>();
                        var employeeNumber = worksheet.Cells[row, 2].GetValue<string>();
                        var fName = worksheet.Cells[row, 3].GetValue<string>();
                        var lName = worksheet.Cells[row, 4].GetValue<string>();
                        var gender = worksheet.Cells[row, 5].GetValue<string>();
                        var phone = worksheet.Cells[row, 7].GetValue<string>();
                        var dob = worksheet.Cells[row, 6].GetValue<DateTime>();
                        
                        var officeEmail = worksheet.Cells[row, 8].GetValue<string>();
                        var mobile = worksheet.Cells[row, 9].GetValue<string>();
                        var personalEmail = worksheet.Cells[row, 10].GetValue<string>();
                        var employeeType = worksheet.Cells[row, 11].GetValue<bool>();
                        var experience = worksheet.Cells[row, 12].GetValue<byte>();
                        var designationId = worksheet.Cells[row, 13].GetValue<byte>();
                        var isInProject = worksheet.Cells[row, 14].GetValue<bool>();
                        var isUpskilling = worksheet.Cells[row, 15].GetValue<bool>();
                        var isWorkingOnInternalTool = worksheet.Cells[row, 16].GetValue<bool>();
                        var projectId = worksheet.Cells[row, 17].GetValue<byte>();
                        var notesId = worksheet.Cells[row, 18].GetValue<int>();
                        var isFirstLogin = worksheet.Cells[row, 19].GetValue<bool>();
                        var deptId = worksheet.Cells[row, 20].GetValue<byte>();
                        var createdBy = worksheet.Cells[row,21].GetValue<int>();
                        var createdOn = worksheet.Cells[row, 22].GetValue<DateTime>();
                        var modifiedBy = worksheet.Cells[row, 23].GetValue<int>();
                        var modifiedOn = worksheet.Cells[row, 24].GetValue<DateTime>();
                        var joinedOn = worksheet.Cells[row, 25].GetValue<DateTime>();
                        var location = worksheet.Cells[row, 26].GetValue<string>();
                        var isActiveColumn = worksheet.Cells[row, 27].GetValue<bool>();
                     //   var password = worksheet.Cells[row, 28].GetValue<byte[]>();

                        // Create a new row in the DataTable
                        var dataRow = table.NewRow();
                        dataRow["Id"] = id;
                        dataRow["EmployeeNumber"] = employeeNumber;
                        dataRow["FName"] = fName;
                        dataRow["LName"] = lName;
                        dataRow["Gender"] = gender;
                        dataRow["DOB"] = dob;
                        dataRow["Phone"] = phone;
                        dataRow["OfficeEmail"] = officeEmail;
                        dataRow["Mobile"] = mobile;
                        dataRow["PersonalEmail"] = personalEmail;
                        dataRow["EmployeeType"] = employeeType;
                        dataRow["Experience"] = experience;
                        dataRow["DesignationId"] = designationId;
                        dataRow["IsInProject"] = isInProject;
                        dataRow["IsUpskilling"] = isUpskilling;
                        dataRow["IsWorkingOnInternalTool"] = isWorkingOnInternalTool;
                        dataRow["ProjectId"] = projectId;
                        dataRow["NotesId"] = notesId;
                        dataRow["IsFirstLogin"] = isFirstLogin;
                        dataRow["DeptId"] = deptId;
                        dataRow["CreatedBy"] = createdBy;
                        dataRow["CreatedOn"] = createdOn;
                        dataRow["ModifiedBy"] = modifiedBy;
                        dataRow["ModifiedOn"] = modifiedOn;
                        dataRow["JoinedOn"] = joinedOn;
                        dataRow["Location"] = location;
                       dataRow["IsActive"] = isActiveColumn;
                    //    dataRow["Password"] = password;
                        table.Rows.Add(dataRow);
                    }

                    // Use SqlBulkCopy to insert the data into the database
                    using (var bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.DestinationTableName = "users";
                        bulkCopy.WriteToServer(table);
                    }
                }
            }

            Console.WriteLine("Data inserted");

        }

        public void OleDateConvo(DateTime date)
        {
            DateTime dateTime = date;
            double oleAuto = dateTime.ToOADate();

            Console.WriteLine(oleAuto);
        }

    }
}

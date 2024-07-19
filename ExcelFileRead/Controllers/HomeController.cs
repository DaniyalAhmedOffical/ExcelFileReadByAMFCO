
using ExcelDataReader;
using ExcelFileRead.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Diagnostics;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
namespace ExcelFileRead.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ExcelFileRead()
        {
            return View();
        }


        public IActionResult Index()
        {

            return View(new UploadViewModel());
        }

        //        [HttpPost]
        //        public async Task<IActionResult> UploadFiles(IFormFile file1, IFormFile file2)
        //        {
        //            var viewModel = new UploadViewModel();

        //            if (file1 != null && file1.Length > 0)
        //            {
        //                await ProcessFile(file1, viewModel, 1);
        //            }

        //            if (file2 != null && file2.Length > 0)
        //            {
        //                await ProcessFile(file2, viewModel, 2);
        //            }

        //            return PartialView("UploadedData", viewModel);
        //        }

        //        private async Task ProcessFile(IFormFile file, UploadViewModel viewModel, int fileNumber)
        //        {
        //            string fileExtension = Path.GetExtension(file.FileName).ToLower();
        //            MemoryStream stream = new MemoryStream();

        //            if (fileExtension == ".csv")
        //            {
        //                await file.CopyToAsync(stream);
        //                stream.Position = 0;

        //                using (var reader = new StreamReader(stream, Encoding.UTF8))
        //                {
        //                    using (var package = new ExcelPackage())
        //                    {
        //                        var worksheet = package.Workbook.Worksheets.Add("Sheet1");

        //                        string line;
        //                        int row = 1;
        //                        while ((line = reader.ReadLine()) != null)
        //                        {
        //                            char separator = line.Contains("|") ? '|' : ',' : ':';
        //                            var values = line.Split(separator);

        //                            for (int col = 0; col < values.Length; col++)
        //                            {
        //                                worksheet.Cells[row, col + 1].Value = values[col];
        //                            }

        //                            row++;
        //                        }

        //                        stream = new MemoryStream();
        //                        package.SaveAs(stream);
        //                        stream.Position = 0;
        //                    }
        //                }
        //            }
        //            else if (fileExtension == ".xlsx" || fileExtension == ".xls")
        //            {
        //                await file.CopyToAsync(stream);
        //                stream.Position = 0;
        //            }

        //            using (var package = new ExcelPackage(stream))
        //            {
        //                var worksheet = package.Workbook.Worksheets[0];
        //                var columnNames = fileNumber == 1 ? viewModel.File1ColumnNames : viewModel.File2ColumnNames;
        //                var rows = fileNumber == 1 ? viewModel.File1Rows : viewModel.File2Rows;

        //                Read column names
        //                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
        //                {
        //                    columnNames.Add(worksheet.Cells[1, col].Text);
        //                }

        //                Read rows
        //                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        //                {
        //                    var rowData = new List<string>();
        //                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
        //                    {
        //                        rowData.Add(worksheet.Cells[row, col].Text);
        //                    }
        //                    rows.Add(rowData);
        //                }
        //            }
        //        }
        //    }
        //}


        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile file1, IFormFile file2)
        {
            var viewModel = new UploadViewModel();

            if (file1 != null && file1.Length > 0)
            {
                await ProcessFile(file1, viewModel, 1);
            }

            if (file2 != null && file2.Length > 0)
            {
                await ProcessFile(file2, viewModel, 2);
            }

            return PartialView("UploadedData", viewModel);
        }

        private async Task ProcessFile(IFormFile file, UploadViewModel viewModel, int fileNumber)
        {
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            MemoryStream stream = new MemoryStream();

            if (fileExtension == ".csv" || fileExtension == ".txt")
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                        string line;
                        int row = 1;
                        while ((line = reader.ReadLine()) != null)
                        {
                            // Define the possible separators
                            char[] separators = new char[] { '|', ',', ';', '?', '/', '!', ':', '^', '~', '`','\t',' '};
                            string[] values = line.Split(separators, StringSplitOptions.None);

                            for (int col = 0; col < values.Length; col++)
                            {
                                worksheet.Cells[row, col + 1].Value = values[col];
                            }

                            row++;
                        }

                        stream = new MemoryStream();
                        package.SaveAs(stream);
                        stream.Position = 0;
                    }
                }
            }

            else if (fileExtension == ".xlsx" || fileExtension == ".xls")
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;
            }

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var columnNames = fileNumber == 1 ? viewModel.File1ColumnNames : viewModel.File2ColumnNames;
                var rows = fileNumber == 1 ? viewModel.File1Rows : viewModel.File2Rows;

                // Read column names
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    columnNames.Add(worksheet.Cells[1, col].Text);
                }

                // Read rows
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var rowData = new List<string>();
                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        rowData.Add(worksheet.Cells[row, col].Text);
                    }
                    rows.Add(rowData);
                }
            }
        }
    }
}

        //private async Task ProcessFile(IFormFile file, UploadViewModel viewModel, int fileNumber)
        //{
        //    string fileExtension = Path.GetExtension(file.FileName).ToLower();
        //    MemoryStream stream = new MemoryStream();

        //    if (fileExtension == ".csv")
        //    {
        //        await file.CopyToAsync(stream);
        //        stream.Position = 0;

        //        using (var reader = new StreamReader(stream, Encoding.UTF8))
        //        {
        //            using (var package = new ExcelPackage())
        //            {
        //                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

        //                string line;
        //                int row = 1;
        //                while ((line = reader.ReadLine()) != null)
        //                {
        //                    // Define the possible separators
        //                    char[] separators = new char[] { '|', ',', ';', '?', '/', '!', ':', '^', '~', '`', '\t' };
        //                    string[] values = line.Split(separators, StringSplitOptions.None);

        //                    for (int col = 0; col < values.Length; col++)
        //                    {
        //                        worksheet.Cells[row, col + 1].Value = values[col];
        //                    }

        //                    row++;
        //                }

        //                stream = new MemoryStream();
        //                package.SaveAs(stream);
        //                stream.Position = 0;
        //            }
        //        }
        //    }
        //    else if (fileExtension == ".xlsx" || fileExtension == ".xls" || fileExtension == ".txt")
        //    {
        //        await file.CopyToAsync(stream);
        //        stream.Position = 0;
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Unsupported file extension");
        //    }

        //    using (var package = new ExcelPackage(stream))
        //    {
        //        var worksheet = package.Workbook.Worksheets[0];
        //        var columnNames = fileNumber == 1 ? viewModel.File1ColumnNames : viewModel.File2ColumnNames;
        //        var rows = fileNumber == 1 ? viewModel.File1Rows : viewModel.File2Rows;

        //        // Read column names
        //        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
        //        {
        //            columnNames.Add(worksheet.Cells[1, col].Text);
        //        }

        //        // Read rows
        //        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        //        {
        //            var rowData = new List<string>();
        //            for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
        //            {
        //                rowData.Add(worksheet.Cells[row, col].Text);
        //            }
        //            rows.Add(rowData);
        //        }
        //    }
        //}
  
using Microsoft.AspNetCore.Mvc;

namespace ExcelFileRead.Models
{
    public class UploadViewModel
    {
        public List<string> File1ColumnNames { get; set; }
        public List<List<string>> File1Rows { get; set; }

        public List<string> File2ColumnNames { get; set; }
        public List<List<string>> File2Rows { get; set; }
     

        public UploadViewModel()
        {

   
            File1ColumnNames = new List<string>();
            File1Rows = new List<List<string>>();

            File2ColumnNames = new List<string>();
            File2Rows = new List<List<string>>();
        }
    }
}

using ClosedXML.Excel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectWPF.StudentManage.Services
{
    public class ExcelExportService
    {
        public void ExportToExcel<T>(IEnumerable<T> data, string filePath)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet1");
            var properties = typeof(T).GetProperties();
            // Header
            for (int i = 0; i < properties.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = properties[i].Name;
            }
            // Data
            int row = 2;
            foreach (var item in data)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cell(row, i + 1).Value = properties[i].GetValue(item)?.ToString() ?? "";
                }
                row++;
            }
            workbook.SaveAs(filePath);
        }
    }
} 
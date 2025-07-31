using CarService.Core.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CarService.Application.Services
{
    public class ExcelGenerationService : IExcelGenerationService
    {
        public async Task<MemoryStream> GenerateOrderRequestExcel(Guid requestId, List<Part> parts, List<Work> works, List<int> partQuantities)
        {
            ExcelPackage.License.SetNonCommercialPersonal("myname");

            var memoryStream = new MemoryStream();
            using (var package = new ExcelPackage(memoryStream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Наряд-заказ");

                worksheet.Cells[1, 1].Value = "Заголовок";
                worksheet.Cells[1, 2].Value = "Наряд-заказ";
                worksheet.Cells[2, 1].Value = "Дата";
                worksheet.Cells[2, 2].Value = DateTime.UtcNow.ToString("dd.MM.yyyy");

                worksheet.Cells[4, 1].Value = "Перечень деталей для ремонта";
                worksheet.Cells[5, 1].Value = "№";
                worksheet.Cells[5, 2].Value = "Наименование";
                worksheet.Cells[5, 3].Value = "Оригин. номер";
                worksheet.Cells[5, 4].Value = "Количество";
                worksheet.Cells[5, 5].Value = "Цена";
                worksheet.Cells[5, 6].Value = "Сумма";

                using (var range = worksheet.Cells[5, 1, 5, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                }

                int row = 6;
                for (int i = 0; i < parts.Count; i++)
                {
                    worksheet.Cells[row, 1].Value = row - 5;
                    worksheet.Cells[row, 2].Value = parts[i].Name;
                    worksheet.Cells[row, 3].Value = parts[i].Article;
                    worksheet.Cells[row, 4].Value = partQuantities[i];
                    worksheet.Cells[row, 5].Value = parts[i].Cost;
                    worksheet.Cells[row, 6].Formula = $"D{row}*E{row}";
                    row++;
                }

                worksheet.Cells[row, 5].Value = "Итого:";
                worksheet.Cells[row, 6].Formula = $"SUM(F6:F{row - 1})";

                worksheet.Cells[row, 5, row, 6].Style.Font.Bold = true;

                worksheet.Cells[row + 2, 1].Value = "Перечень производимых работ";
                worksheet.Cells[row + 3, 1].Value = "№";
                worksheet.Cells[row + 3, 2].Value = "Наименование";
                worksheet.Cells[row + 3, 3].Value = "Оригин. номер";
                worksheet.Cells[row + 3, 4].Value = "Цена";

                row += 3;
                for (int i = 0; i < works.Count; i++)
                {
                    worksheet.Cells[row, 1].Value = row - 8;
                    worksheet.Cells[row, 2].Value = works[i].Name;
                    worksheet.Cells[row, 3].Value = works[i].Description;
                    worksheet.Cells[row, 4].Value = works[i].Cost;
                    row++;
                }

                worksheet.Cells[row, 3].Value = "Итого:";
                worksheet.Cells[row, 4].Formula = $"SUM(D{row - works.Count}:D{row - 1})";

                worksheet.Cells[row, 3, row, 4].Style.Font.Bold = true;

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                worksheet.Column(1).Width = 10; 
                worksheet.Column(2).Width = 30; 
                worksheet.Column(3).Width = 20; 
                worksheet.Column(4).Width = 15; 
                worksheet.Column(5).Width = 15;

                package.Save();
            }

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}

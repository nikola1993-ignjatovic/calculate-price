﻿using CalculatePrice.Dtos;
using CalculatePrice.Enums;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data;
using System.Drawing;

namespace CalculatePrice.Services
{
    public class DocumentService : IDisposable, IDocumentService
    {
        private XLWorkbook _workbook;
        private bool _disposed = false;
        public DocumentService()
        {
            _workbook = new XLWorkbook();
        }
        public void CreateNewSheet(string sheetName)
        {
            if (_workbook.Worksheets.Contains(sheetName))
            {
                System.Console.WriteLine($"Sheet '{sheetName}' already exists.");
                return;
            }
            var worksheet = _workbook.Worksheets.Add(sheetName);
            HeaderStyling(worksheet, 4, 1, 4, 11);
            HeaderStyling(worksheet, 1, 1, 1, 13);
        }
        private void HeaderStyling(IXLWorksheet worksheet, int firstCellRow, int firstCellColumn, int lastCellRow, int lastCellColumn)
        {
            var range = worksheet.Range(firstCellRow, firstCellColumn, lastCellRow, lastCellColumn); 
            range.Style.Fill.BackgroundColor = XLColor.Blue;
            range.Style.Font.Bold = true;
            range.Style.Font.FontColor = XLColor.White; 
            range.Style.Font.FontSize = 14;
        }
        public void AddDataToSheet(DataTable dataTable, string sheetName)
        {
            if (!_workbook.Worksheets.Contains(sheetName))
            {
                Console.WriteLine($"Sheet '{sheetName}' does not exist. Creating the sheet.");
                CreateNewSheet(sheetName);
            }

            var worksheet = _workbook.Worksheet(sheetName);

            for (int col = 1; col <= dataTable.Columns.Count; col++)
            {
                worksheet.Cell(1, col).Value = dataTable.Columns[col - 1].ColumnName;
            }

            for (int row = 1; row <= dataTable.Rows.Count; row++)
            {
                for (int col = 1; col <= dataTable.Columns.Count; col++)
                {
                    var cellValue = dataTable.Rows[row - 1][col - 1] != null ? dataTable.Rows[row - 1][col - 1].ToString() : string.Empty;
                    worksheet.Cell(row + 1, col).Value = cellValue;
                }
            }
        }
        public void SaveWorkbook(string filePath)
        {
            _workbook.SaveAs(filePath);
            System.Console.WriteLine($"Workbook saved to '{filePath}'.");
        }
        public List<ProductDto> GetAllProducts(string filePath, byte sheetNumber = 1)
        {
            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(sheetNumber);

                var rows = worksheet.RowsUsed().Skip(1).ToList() ; //skip header               
                var products = new List<ProductDto>();
                //:TODO Nikola VALIDATION FOR ALL FIELDS      
                rows.ForEach(row =>
                    products.Add(new ProductDto()
                    {
                        Caption = row.Cell((int)ProductColumnType.Caption).GetString(),
                        Price = row.Cell((int)ProductColumnType.Price).GetDouble(),
                        RaiseOrLower = row.Cell((int)ProductColumnType.RaiseOrLower).GetDouble()
                    })
                );
                return products;
            }
        }
        public List<ProductTierRateDto> GetAllProductTierRate(string filePath, byte sheetNumber = 1)
        {
            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(sheetNumber);

                var rows = worksheet.RowsUsed().Skip(1).ToList(); //skip header               
                var productTierRateDtos = new List<ProductTierRateDto>();
                //:TODO Nikola VALIDATION FOR ALL FIELDS      
                rows.ForEach(row =>
                    productTierRateDtos.Add(new ProductTierRateDto()
                    {
                        Broker = row.Cell((int)ProductTierRateColumnType.Broker).GetString(),
                        Symbol = row.Cell((int)ProductTierRateColumnType.Symbol).GetString(),
                        LongCaption = row.Cell((int)ProductTierRateColumnType.LongCaption).GetString(),
                        OrderType = row.Cell((int)ProductTierRateColumnType.OrderType).GetString(),
                        Low = row.Cell((int)ProductTierRateColumnType.Low).GetDouble(),
                        High = row.Cell((int)ProductTierRateColumnType.High).GetDouble(),
                        Rate = row.Cell((int)ProductTierRateColumnType.Rate).GetDouble(),
                        Location = row.Cell((int)ProductTierRateColumnType.Location).GetString()
                    })
                );
                return productTierRateDtos;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here, if any
                }

                _disposed = true;
            }
        }

    }
}
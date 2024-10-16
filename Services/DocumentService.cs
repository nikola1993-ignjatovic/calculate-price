using CalculatePrice.Dtos;
using CalculatePrice.Enums;
using CalculatePrice.Helpers;
using ClosedXML.Excel;
using System.Data;
using Helper =  CalculatePrice.Helpers.Helper;
namespace CalculatePrice.Services
{
    public class DocumentService : IDisposable, IDocumentService
    {
        private XLWorkbook _workbook;
        private bool _disposed = false;
        private string InputfileProductsPath { get; set; }
        private string InputfileTiersPath { get; set; }
        private string OutputFolderPath { get; set; }
        public DocumentService()
        {
            _workbook = new XLWorkbook();
            OutputFolderPath = @"C:\Users\Nikola.Ignjatovic\Documents";
            InputfileProductsPath = @"C:\Users\Nikola.Ignjatovic\Downloads\2024-10-04 HAA Buy Price Adjust (1).xlsx"; //read from settings
            InputfileTiersPath = @"C:\Users\Nikola.Ignjatovic\Downloads\BrokerTierRates.xlsx"; //read from settings
            //create folders for TS and products excel files
            CreateNewFolder(OutputFolderPath, Constants.FolderNames.Products);
            CreateNewFolder(OutputFolderPath, Constants.FolderNames.TestSuiteCommands);
        }
        private void CreateNewFolder(string path, string folderName)
        {
            var folderPath = $"{path}\\{folderName}";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            else
            {
                Directory.GetFiles(folderPath).ToList().ForEach(file 
                                                                    => File.Delete(file)
                );                
            }
        }
        [Obsolete]
        public void CreateNewSheetOld(string sheetName, byte numberOfLocations)
        {
            sheetName = Helper.FixSheetName(sheetName);
            if (_workbook.Worksheets.Contains(sheetName))
            {
                System.Console.WriteLine($"Sheet '{sheetName}' already exists.");
                return;
            }
            var worksheet = _workbook.Worksheets.Add(sheetName);
            HeaderStyling(worksheet, 1, 1, 1, Constants.NumberOfColumnsBaseRate);
            HeaderStyling(worksheet, numberOfLocations + Constants.NumberofUsedRows, 1, numberOfLocations + Constants.NumberofUsedRows, Constants.NumberOfColumnsAllRates);
        }
        public void CreateNewSheet(XLWorkbook workbook, string sheetName)
        {
            sheetName = Helper.FixSheetName(sheetName);
            if (workbook.Worksheets.Contains(sheetName))
            {
                System.Console.WriteLine($"Sheet '{sheetName}' already exists.");
                return;
            }
            var worksheet = workbook.Worksheets.Add(sheetName);
            var styledRow = sheetName.Contains(Constants.TSPrefix) ? Constants.WorksheetTestsuiteStyledRow : Constants.WorksheetStyledRow;
            HeaderStyling(worksheet, styledRow, 1, styledRow, Constants.NumberOfColumnsBaseRate);
        }
        private void HeaderStyling(IXLWorksheet worksheet, int firstCellRow, int firstCellColumn, int lastCellRow, int lastCellColumn)
        {
            var range = worksheet.Range(firstCellRow, firstCellColumn, lastCellRow, lastCellColumn);
            range.Style.Fill.BackgroundColor = XLColor.Blue;
            range.Style.Font.Bold = true;
            range.Style.Font.FontColor = XLColor.White;
            range.Style.Font.FontSize = 14;
        }
        public void AddDataToSheetOld(DataTable dataTable, string sheetName, byte numberOfLocations)
        {
            sheetName = Helper.FixSheetName(sheetName);
            if (!_workbook.Worksheets.Contains(sheetName))
            {
                Console.WriteLine($"Sheet '{sheetName}' does not exist. Creating the sheet.");
                CreateNewSheetOld(sheetName, numberOfLocations);
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
        public void AddDataToSheet(XLWorkbook workbook, DataTable dataTable, string sheetName)
        {
            sheetName = Helper.FixSheetName(sheetName);
            if (!workbook.Worksheets.Contains(sheetName))
            {
                Console.WriteLine($"Sheet '{sheetName}' does not exist. Creating the sheet.");
                CreateNewSheet(workbook, sheetName);
            }

            var worksheet = workbook.Worksheet(sheetName);

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
        public void SaveWorkBook(XLWorkbook workbook, string outputFileName)
        {
            var outputFolder = outputFileName.Contains(Constants.TSPrefix) ? Constants.FolderNames.TestSuiteCommands : Constants.FolderNames.Products;
            var outputFilePath = $"{OutputFolderPath}\\{outputFolder}\\{outputFileName}";
            workbook.SaveAs(outputFilePath);
        }
        public List<ProductDto> GetAllProducts(byte sheetNumber = 1)
        {
            using (var workbook = new XLWorkbook(InputfileProductsPath))
            {
                var worksheet = workbook.Worksheet(sheetNumber);

                var rows = worksheet.RowsUsed().Skip(1).ToList(); //skip header               
                var products = new List<ProductDto>();
                rows.ForEach(row =>
                    products.Add(new ProductDto()
                    {
                        Caption = row.Cell((int)ProductColumnType.Caption).GetString(),
                        Price = row.Cell((int)ProductColumnType.Price).GetDouble(),
                        RaiseOrLower = row.Cell((int)ProductColumnType.RaiseOrLower).GetDouble(),
                        Symbol = row.Cell((int)ProductColumnType.Symbol).GetString(),
                    })
                );
                return products;
            }
        }
        public List<ProductTierRateDto> GetAllProductTierRate(byte sheetNumber = 1)
        {
            using (var workbook = new XLWorkbook(InputfileTiersPath))
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
                        OrderType = (OrderType)Enum.Parse(typeof(OrderType), row.Cell((int)ProductTierRateColumnType.OrderType).GetString()),
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
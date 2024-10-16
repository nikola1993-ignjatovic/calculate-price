using CalculatePrice.Dtos;
using ClosedXML.Excel;
using System.Data;

namespace CalculatePrice.Services
{
    public interface IDocumentService
    {
        void CreateNewSheetOld(string sheetName, byte numberOfLocations);
        void CreateNewSheet(XLWorkbook workbook, string sheetName);
        void AddDataToSheetOld(DataTable dataTable, string sheetName, byte numberOfLocations);
        void AddDataToSheet(XLWorkbook workbook, DataTable dataTable, string sheetName);
        List<ProductDto> GetAllProducts(byte sheetNumber = 1);
        List<ProductTierRateDto> GetAllProductTierRate(byte sheetNumber = 1);
        void SaveWorkBook(XLWorkbook workbook, string outputFileName);
    }
}

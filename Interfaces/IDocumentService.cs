using CalculatePrice.Dtos;
using ClosedXML.Excel;
using System.Data;

namespace CalculatePrice.Services
{
    public interface IDocumentService
    {
        void CreateNewSheet(string sheetName);
        void AddDataToSheet(DataTable dataTable, string sheetName);
        List<ProductDto> GetAllProducts(string filePath, byte sheetNumber = 1);
        List<ProductTierRateDto> GetAllProductTierRate(string filePath, byte sheetNumber = 1);
        void SaveWorkbook(string filePath);
    }
}

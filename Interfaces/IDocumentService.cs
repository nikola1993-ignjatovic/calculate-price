using CalculatePrice.Dtos;
using System.Collections.Generic;
using System.Data;

namespace CalculatePrice.Services
{
    public interface IDocumentService
    {
        void CreateNewSheetOld(string sheetName, byte numberOfLocations);
        void CreateNewSheet(string sheetName);
        void AddDataToSheetOld(DataTable dataTable, string sheetName, byte numberOfLocations);
        void AddDataToSheet(DataTable dataTable, string sheetName);
        List<ProductDto> GetAllProducts(byte sheetNumber = 1);
        List<ProductTierRateDto> GetAllProductTierRate(byte sheetNumber = 1);
        void SaveWorkbook();
    }
}

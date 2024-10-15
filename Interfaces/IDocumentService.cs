﻿using CalculatePrice.Dtos;
using System.Collections.Generic;
using System.Data;

namespace CalculatePrice.Services
{
    public interface IDocumentService
    {
        void CreateNewSheet(string sheetName);
        void AddDataToSheet(DataTable dataTable, string sheetName);
        List<ProductDto> GetAllProducts(byte sheetNumber = 1);
        List<ProductTierRateDto> GetAllProductTierRate(byte sheetNumber = 1);
        void SaveWorkbook();
    }
}

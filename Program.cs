using CalculatePrice.Dtos;
using CalculatePrice.Helpers;
using CalculatePrice.Interfaces;
using CalculatePrice.Services;
using System.Configuration;
string filePath = @"C:\Users\Nikola.Ignjatovic\Documents\sample_data.xlsx";
string inputfileProductsPath = @"C:\Users\Nikola.Ignjatovic\Downloads\2024-10-04 HAA Buy Price Adjust (1).xlsx";
string inputfileTiersPath = @"C:\Users\Nikola.Ignjatovic\Downloads\BrokerTierRates.xlsx";

using (var docService= new DocumentService())
{
    using (var calcualtionService = new CalculationService(docService))
    {    
        calcualtionService.BeginCalculation(inputfileProductsPath, inputfileTiersPath);
        var exportRows = calcualtionService.PerformCalculation();
        using (var tableService = new TableService())
        {
            string sheetName = "Sheet1";
            docService.CreateNewSheet(sheetName);

            tableService
                        .AddHeader()
                        .AddRows(exportRows.Take(Constants.NumberOfRowForFirstTier).ToList())
                        .AddSecondHeader()
                        .AddRows(exportRows.Skip(Constants.NumberOfRowForFirstTier).ToList());

            docService.AddDataToSheet(tableService.GetTable(), sheetName);

            docService.SaveWorkbook(filePath);
        }
    }
}

using CalculatePrice.Helpers;
using CalculatePrice.Services;


using (var docService = new DocumentService())
{
    using (var calcualtionService = new CalculationService(docService))
    {
        calcualtionService.BeginCalculation();
        var exportRows = calcualtionService.PerformCalculation();
        foreach (var row in exportRows)
        {
            using (var tableService = new TableService())
            {          
                string sheetName = row.Key;
                docService.CreateNewSheet(sheetName);
                var productRows = row.Value;

                tableService
                            .AddHeader()
                            .AddRows(productRows.Take(Constants.NumberOfRowForFirstTier).ToList())
                            .AddSecondHeader()
                            .AddRows(productRows.Skip(Constants.NumberOfRowForFirstTier).ToList());

                docService.AddDataToSheet(tableService.GetTable(), sheetName);
            }
        }
        docService.SaveWorkbook();
    }
}

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
                if (row.Value != null && row.Value.ExportBaseTiersRows.Any()) //&& row.Value.ExportAllTiersRows.Any())
                {
                    docService.CreateNewSheet(sheetName); //docService.CreateNewSheetOld(sheetName, row.Value.NumberOfLocations);
                    tableService
                        .AddHeader()
                        .AddRows(row.Value.ExportBaseTiersRows);
                        //.AddSecondHeader()
                        //.AddRows(row.Value.ExportAllTiersRows);
                    docService.AddDataToSheet(tableService.GetTable(), sheetName); // docService.AddDataToSheetOld(tableService.GetTable(), sheetName, row.Value.NumberOfLocations);
                }

            }
        }
        docService.SaveWorkbook();
    }
}

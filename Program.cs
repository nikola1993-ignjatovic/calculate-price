using CalculatePrice.Services;
using ClosedXML.Excel;
using Constants = CalculatePrice.Helpers.Constants;

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
                var isTestSuite = sheetName.Contains(Constants.TSPrefix);
                if (row.Value != null && row.Value.Any()) 
                {
                    var workBook = new XLWorkbook();
                    docService.CreateNewSheet(workBook, sheetName);
                    if (!isTestSuite)
                    {
                        tableService
                            .AddHeader(row.Key)
                            .AddRows(row.Value, isTestSuite);
                    }
                    else
                    {
                        tableService
                            .Init(sheetName)
                            .AddSubHeader(Constants.SetBroker, Constants.BrokerCode)
                            .AddHeader(row.Key)
                            .AddRows(row.Value, isTestSuite);
                    }
                    var dataTable =  tableService.GetTable();
                    docService.AddDataToSheet(workBook, dataTable, sheetName);
                    docService.SaveWorkBook(workBook, $"{sheetName}.xlsx");
                }
            }
        }      
    }
}

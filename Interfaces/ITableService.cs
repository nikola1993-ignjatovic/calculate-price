using System.Data;
using CalculatePrice.Dtos;

namespace CalculatePrice.Services
{
    public interface ITableService
    {
        ITableService Init(string sheetName);
        ITableService AddHeader(string sheetName);
        ITableService AddSecondHeader();
        ITableService AddSubHeader(string key, string value);
        ITableService AddRows(List<ExportRowBaseDto> rows, bool isTestSuite);
        DataTable GetTable();
    }
}

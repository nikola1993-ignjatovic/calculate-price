using System.Collections.Generic;
using System.Data;
using CalculatePrice.Dtos;

namespace CalculatePrice.Services
{
    public interface ITableService
    {
        ITableService AddHeader();
        ITableService AddSecondHeader();
        ITableService AddRows(List<ExportRowBaseDto> rows);
        DataTable GetTable();
    }
}

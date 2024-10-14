using CalculatePrice.Dtos;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CalculatePrice.Services
{
    public class TableService : IDisposable, ITableService
    {
        private DataTable _dataTable;
        private bool _disposed = false;
        public TableService()
        {
            _dataTable = new DataTable();
        }
        public ITableService AddHeader()
        {
            var properties = typeof(ExportRowBaseDto).GetProperties().ToList();
            //properties.ForEach(property =>
            //   _dataTable.Columns.Add(property.Name, 
            //                          property.PropertyType));

            _dataTable.Columns.Add(nameof(ExportRowBaseDto.Symbol));
            _dataTable.Columns.Add(nameof(ExportRowBaseDto.OrderType));
            _dataTable.Columns.Add(nameof(ExportRowBaseDto.Low));
            _dataTable.Columns.Add(nameof(ExportRowBaseDto.High));
            _dataTable.Columns.Add(nameof(ExportRowBaseDto.BrokerRate));
            _dataTable.Columns.Add(nameof(ExportRowBaseDto.Location));
            _dataTable.Columns.Add(nameof(ExportRowBaseDto.ReferentPrice));
            _dataTable.Columns.Add(nameof(ExportRowBaseDto.ClientPrice));
            _dataTable.Columns.Add(nameof(ExportRowBaseDto.Discount));
            _dataTable.Columns.Add(nameof(ExportRowBaseDto.DesiredClientPrice));
            _dataTable.Columns.Add(nameof(ExportRowBaseDto.NewBrokerRate));
            _dataTable.Columns.Add(nameof(ExportRowBaseDto.DiscountOnRate));
            _dataTable.Columns.Add(nameof(ExportRowBaseDto.Check));
            return this;
        }
        public ITableService AddSecondHeader()
        {
            var properties = typeof(ExportRowBaseDto).GetProperties().ToList();
            //properties.ForEach(property =>
            //   _dataTable.Columns.Add(property.Name, 
            //                          property.PropertyType));

            _dataTable.Rows.Add(nameof(ExportRowBaseDto.Symbol), 
                nameof(ExportRowBaseDto.OrderType),
                nameof(ExportRowBaseDto.Low), 
                nameof(ExportRowBaseDto.High),
                nameof(ExportRowBaseDto.BrokerRate), 
                nameof(ExportRowBaseDto.Location), 
                nameof(ExportRowBaseDto.ReferentPrice),
                nameof(ExportRowBaseDto.ClientPrice),
                nameof(ExportRowBaseDto.Discount), 
                nameof(ExportRowBaseDto.NewBrokerRate),
                nameof(ExportRowBaseDto.Check));
            return this;
        }
        public ITableService AddRows(List<ExportRowBaseDto> rows)
        {
            rows.ForEach(row =>
                       AddRow(row));
            return this;
        }
        private void AddRow(ExportRowBaseDto row)
        {
            if (row is null)
                _dataTable.Rows.Add();
            else
            {

                if (row.ShowFirstRow)
                {
                    _dataTable.Rows.Add(row.Symbol,
                                        row.OrderType,
                                        row.Low,
                                        row.High,
                                        row.BrokerRate,
                                        row.Location,
                                        row.ReferentPrice,
                                        row.ClientPrice,
                                        row.Discount,
                                        row.NewBrokerRate,
                                        row.DiscountOnRate,
                                        row.Check);
                }
                else
                {
                    _dataTable.Rows.Add(row.Symbol,
                                        row.OrderType,
                                        row.Low,
                                        row.High,
                                        row.BrokerRate,
                                        row.Location,
                                        row.ReferentPrice,
                                        row.ClientPrice,
                                        row.Discount,
                                        row.NewBrokerRate,
                                        row.Check);
                }
            }
        }
        public DataTable GetTable() => _dataTable;        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here, if any
                }
                _disposed = true;
            }
        }
    }
}

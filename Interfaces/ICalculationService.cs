using CalculatePrice.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatePrice.Interfaces
{
    public interface ICalculationService
    {
        void BeginCalculation(string fileProductsPath, string fileTiersPath);
        List<ExportRowBaseDto> PerformCalculation();

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatePrice.Dtos
{
    public class ExportProductDto
    {
        public byte NumberOfLocations { get; set; }
         
        public List<ExportRowBaseDto> ExportBaseTiersRows { get; set; }
        public List<ExportRowBaseDto> ExportAllTiersRows { get; set; }
    }
}

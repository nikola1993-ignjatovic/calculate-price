using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatePrice.Dtos
{
    public class ProductDto
    {
        public string Caption { get; set; }
        public double Price { get; set; }
        public double RaiseOrLower { get; set; }
        public string Symbol {  get; set; }
    }
}

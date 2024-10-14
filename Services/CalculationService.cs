using CalculatePrice.Dtos;
using CalculatePrice.Enums;
using CalculatePrice.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatePrice.Services
{
    public class CalculationService : ICalculationService, IDisposable
    {
        private bool _disposed = false;
        IDocumentService _documentService;
        private List<ProductDto> products;
        private List<ProductTierRateDto> productTierRateDtos;
        public CalculationService(IDocumentService documentService)
        {
            _documentService = documentService;
            products = new List<ProductDto>();
        }
        public void BeginCalculation(string fileProductsPath,string fileTiersPath)
        {
            products = _documentService.GetAllProducts(fileProductsPath);
            productTierRateDtos = _documentService.GetAllProductTierRate(fileTiersPath) //TODO: prevent duplicates, move to Dictionary
                                                                        .OrderBy(x => x.Symbol)
                                                                        .OrderBy(x => x.Location)
                                                                        .ThenBy(x => x.OrderType)
                                                                        .ThenBy(x => x.Low)
                                                                        .ToList();



            foreach (var product in products)
            {
                Console.WriteLine($"Caption: {product.Caption}, Price:{product.Price}, RaiseOrLower:{product.RaiseOrLower}");
            }
            foreach (var tierRateDto in productTierRateDtos)
            {
                Console.WriteLine($"Broker: {tierRateDto.Broker}, Symbol:{tierRateDto.Symbol}, LongCaption:{tierRateDto.LongCaption},  OrderType:{tierRateDto.OrderType}, Low:{tierRateDto.Low}, High:{tierRateDto.High}, Location:{tierRateDto.Location}  ");
            }
        }
        public List<ExportRowBaseDto> PerformCalculation()
        {
            var exportRows = new List<ExportRowBaseDto>();           

            foreach (var product in products.Where(x => x.Caption == "Silver American Eagle (1 oz) Coin"))
            {
                var allTiersByProducts = productTierRateDtos.Where(x => x.LongCaption == product.Caption  //&& product.Caption == "Silver American Eagle (1 oz) Coin"
                                                                                                         && x.OrderType == OrderType.Buy.ToString()).ToList();

                var firstTierNewYork = productTierRateDtos.Where(x => x.LongCaption == product.Caption && x.OrderType == OrderType.Buy.ToString() && x.Location == "New York" && x.Low == 0).FirstOrDefault();
                var referentPrice =  product.Price / (1 +  (firstTierNewYork is not null ? firstTierNewYork.Rate : 0));

                var exportFirstRow = new ExportRowFirstDto()
                {
                    Symbol = firstTierNewYork?.Symbol,
                    OrderType = firstTierNewYork?.OrderType,
                    ClientPrice = product.Price,
                    BrokerRate = firstTierNewYork is not null ? firstTierNewYork.Rate : 0d,
                    Discount = product.RaiseOrLower,
                    Low = firstTierNewYork is not null ? firstTierNewYork.Low : 0,
                    High = firstTierNewYork is not null ? firstTierNewYork.High : 0,
                    Location = firstTierNewYork is not null ? firstTierNewYork?.Location : string.Empty,
                    ReferentPrice = referentPrice,
                    ShowFirstRow = true
                };
                exportRows.Add(exportFirstRow);
                exportRows.Add(null);
                foreach (var tier in allTiersByProducts)
                {
                    var exportRow = new ExportRowDto()
                    {
                        Symbol = tier.Symbol,
                        OrderType = tier.OrderType,
                        ClientPrice = Math.Round(referentPrice * (1 + tier.Rate), 2),
                        ReferentPrice = referentPrice,
                        BrokerRate = tier.Rate,
                        Low = tier.Low,
                        High = tier.High,
                        Location = tier.Location,
                        OldDiscountOnRate = exportFirstRow.DiscountOnRate
                    };
                    exportRows.Add(exportRow);
                }
            }
            return exportRows;
        }
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

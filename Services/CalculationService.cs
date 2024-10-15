using CalculatePrice.Dtos;
using CalculatePrice.Enums;
using CalculatePrice.Interfaces;
using System;

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
            productTierRateDtos = new List<ProductTierRateDto>();
        }
        public void BeginCalculation()
        {
            products = _documentService.GetAllProducts();
            productTierRateDtos = _documentService.GetAllProductTierRate() //TODO: prevent duplicates, move to Dictionary
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
                Console.WriteLine($"Broker: {tierRateDto.Broker}," +
                                  $"Symbol:{tierRateDto.Symbol}, " +
                                  $"LongCaption:{tierRateDto.LongCaption}, " +
                                  $"OrderType:{tierRateDto.OrderType}, " +
                                  $"Low:{tierRateDto.Low}, " +
                                  $"High:{tierRateDto.High}, " +
                                  $"Location:{tierRateDto.Location} ");
            }
        }
        public Dictionary<string, List<ExportRowBaseDto>> PerformCalculation()
        {
            var exportRows = new Dictionary<string, List<ExportRowBaseDto>>();
            foreach (var product in products)
            {
              exportRows.Add(product.Caption, PerformProductCalculation(product));
            }
            return exportRows;
        }
        public List<ExportRowBaseDto> PerformProductCalculation(ProductDto productDto)
        {
            var exportRows = new List<ExportRowBaseDto>();            
            var allTiersByProducts = productTierRateDtos.Where(x => x.LongCaption == productDto.Caption 
                                                                                                        && x.OrderType == OrderType.Buy).ToList();
                
            var locationNewYork = LocationType.NewYork.ToLocationString();
            var firstTierNewYork = productTierRateDtos.Where(x => x.LongCaption == productDto.Caption && x.OrderType == OrderType.Buy && x.Location == locationNewYork && x.Low == 0).FirstOrDefault();
            var referentPrice = productDto.Price / (1 + (firstTierNewYork != null ? firstTierNewYork.Rate : 0));

            var exportFirstRow = new ExportRowFirstDto()
            {
                Symbol = firstTierNewYork?.Symbol,
                OrderType = firstTierNewYork?.OrderType.ToString(),
                ClientPrice = productDto.Price,
                BrokerRate = firstTierNewYork != null ? firstTierNewYork.Rate : 0d,
                Discount = productDto.RaiseOrLower,
                Low = firstTierNewYork != null ? firstTierNewYork.Low : 0,
                High = firstTierNewYork != null ? firstTierNewYork.High : 0,
                Location = firstTierNewYork != null ? firstTierNewYork?.Location : string.Empty,
                ReferentPrice = referentPrice,
                ShowFirstRow = true
            };
            exportRows.Add(exportFirstRow);
            exportRows.Add(null); //empty row between tables
            foreach (var tier in allTiersByProducts)
            {
                var exportRow = new ExportRowDto()
                {
                    Symbol = tier.Symbol,
                    OrderType = tier.OrderType.ToString(),
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

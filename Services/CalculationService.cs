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
            products = _documentService.GetAllProducts().Where(x => x.Caption == "Silver American Eagle (1 oz) Coin").ToList();
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
        public Dictionary<string, ExportProductDto>  PerformCalculation()
        {
            var exportRows = new Dictionary<string, ExportProductDto>();
            foreach (var product in products.GroupBy(x => x.Caption))
            {
              exportRows.Add(product.Key, PerformProductCalculation(product));
            }
            return exportRows;
        }
        [Obsolete]
        private ExportProductDto PerformProductCalculationOld(IGrouping<string, ProductDto> productGroupDto)
        {
            var exportBaseTiersRows = new List<ExportRowBaseDto>();
            var exportAllTiersRows = new List<ExportRowBaseDto>();
            var allTiersByProducts = productTierRateDtos.Where(x => x.LongCaption == productGroupDto.Key && x.OrderType == OrderType.Buy).ToList();

            byte numberOfLocations = 0;
            foreach (var location in allTiersByProducts.Select(x => x.Location).Distinct())
            {
                var firstTiersByLocation = GetTiersRateByLocation(location, allTiersByProducts);
                var firstTierByLocation = firstTiersByLocation.FirstOrDefault(x => x.Low == 0);
                var productByLocation = productGroupDto.FirstOrDefault(x => x.Symbol == firstTierByLocation.Symbol);
                var referentPrice = Helpers.Helper.GetReferentPrice(productByLocation, firstTierByLocation);
                var exportFirstRow = PerformCalculationPerSymbol(firstTierByLocation, productByLocation, referentPrice);
                exportBaseTiersRows.Add(exportFirstRow);

                firstTiersByLocation.ForEach(tier => exportAllTiersRows.Add(new ExportRowDto
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
                }));
                numberOfLocations++;
            }

            exportBaseTiersRows.Add(null); //ADD EMPTY ROW
            return new ExportProductDto() { 
                ExportBaseTiersRows = exportBaseTiersRows,
                ExportAllTiersRows = exportAllTiersRows, 
                NumberOfLocations = numberOfLocations 
            };
        }
        private ExportProductDto PerformProductCalculation(IGrouping<string, ProductDto> productGroupDto)
        {
            var exportBaseTiersRows = new List<ExportRowBaseDto>();
            var allTiersByProducts = productTierRateDtos.Where(x => x.LongCaption == productGroupDto.Key && x.OrderType == OrderType.Buy).ToList();
            foreach (var tier in allTiersByProducts)
            {
                var productByLocation = productGroupDto.FirstOrDefault(x => x.Symbol == tier.Symbol);
                if (productByLocation != null)
                {
                    var referentPrice = Helpers.Helper.GetReferentPrice(productByLocation, tier);
                    var exportFirstRow = PerformCalculationPerSymbol(tier, productByLocation, referentPrice);
                    exportBaseTiersRows.Add(exportFirstRow);
                }
            }

            return new ExportProductDto()
            {
                ExportBaseTiersRows = exportBaseTiersRows                
            };
        }
        private List<ProductTierRateDto> GetTiersRateByLocation(string location, List<ProductTierRateDto> allTiersByProducts)
        {
            return allTiersByProducts?.Where(x => x.Location == location).ToList();
        }
        private ExportRowFirstDto PerformCalculationPerSymbol(ProductTierRateDto productTierRateDto, ProductDto productDto, double referentPrice)
        {
            return new ExportRowFirstDto()
            {
                Symbol = productTierRateDto?.Symbol,
                OrderType = productTierRateDto?.OrderType.ToString(),
                ClientPrice = productDto.Price,
                BrokerRate = productTierRateDto != null ? productTierRateDto.Rate : 0d,
                Discount = productDto.RaiseOrLower,
                Low = productTierRateDto != null ? productTierRateDto.Low : 0,
                High = productTierRateDto != null ? productTierRateDto.High : 0,
                Location = productTierRateDto != null ? productTierRateDto?.Location : string.Empty,
                ReferentPrice = referentPrice,
                ShowFirstRow = true
            };
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

using System.Diagnostics.CodeAnalysis;

namespace Musicalog.Common.Models
{
    [ExcludeFromCodeCoverage]
    public class InventoryModel
    {
        public int Id { get; set; }
        public string SKU { get; set; }
        public int StockPurchased { get; set; }
        public int SoldSoFar { get; set; }
        public int Stock { get; set; }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Musicalog.Common.Models
{
    [ExcludeFromCodeCoverage]
    public class AlbumModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }        
        public List<ArtistModel> Artists { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public AlbumType Type { get; set; }
        public InventoryModel Inventory { get; set; }
    }
}

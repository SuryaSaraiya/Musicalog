using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Musicalog.Common.Models
{
    [ExcludeFromCodeCoverage]
    public class AlbumListResult
    {
        public List<AlbumModel> Albums { get; set; }
        public int Total { get; set; }
    }
}

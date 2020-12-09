using MediatR;
using Musicalog.Common.Models;
using System.Diagnostics.CodeAnalysis;

namespace Musicalog.Common.Infrastructure.RequestHandlers.Albums
{
    [ExcludeFromCodeCoverage]
    public class UpdateAlbumCommand : IRequest<bool>
    {
        public int AlbumId { get; set; }
        public string AlbumName { get; set; }
        public ArtistModel Artist { get; set; }
        public int Stock { get; set; }
        public int Type { get; set; }
    }
}

using MediatR;
using Musicalog.Common.Models;
using System.Diagnostics.CodeAnalysis;

namespace Musicalog.Common.Infrastructure.RequestHandlers.Albums
{
    [ExcludeFromCodeCoverage]
    public class CreateAlbumCommand : IRequest<int>
    {
        public int AlbumId { get; set; }
        public string AlbumName { get; set; }
        public string ArtistName { get; set; }
        public int Stock { get; set; }
        public int Type { get; set; }
    }
}

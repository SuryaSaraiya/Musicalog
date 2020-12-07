using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Musicalog.Common.Infrastructure.RequestHandlers.Albums
{
    [ExcludeFromCodeCoverage]
    public class DeleteAlbumCommand : IRequest<bool>
    {
        public int AlbumId { get; set; }
    }
}

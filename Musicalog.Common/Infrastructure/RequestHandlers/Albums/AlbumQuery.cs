using MediatR;
using Musicalog.Common.Models;
using System.Diagnostics.CodeAnalysis;

namespace Musicalog.Common.Infrastructure.RequestHandlers.Albums
{
    [ExcludeFromCodeCoverage]
    public class AlbumQuery : IRequest<AlbumModel>
    {
        public int Id { get; set; }
    }
}

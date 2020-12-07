using MediatR;
using Musicalog.Common.Models;
using System.Diagnostics.CodeAnalysis;

namespace Musicalog.Common.Infrastructure.RequestHandlers.Albums
{
    [ExcludeFromCodeCoverage]
    public class AlbumListQuery : IRequest<AlbumListResult>
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}

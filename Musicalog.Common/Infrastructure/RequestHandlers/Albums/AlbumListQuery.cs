using MediatR;
using Musicalog.Common.Models;
using System.Diagnostics.CodeAnalysis;

namespace Musicalog.Common.Infrastructure.RequestHandlers.Albums
{
    [ExcludeFromCodeCoverage]
    public class AlbumListQuery : IRequest<AlbumListResult>
    {        
        public int Skip { get; set; }
        public int Take { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
    }
}

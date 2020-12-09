using MediatR;
using Musicalog.Common.Data;
using Musicalog.Common.Data.Models;
using Musicalog.Common.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Musicalog.Common.Infrastructure.RequestHandlers.Albums
{
    public class AlbumListQueryHandler : IRequestHandler<AlbumListQuery, AlbumListResult>
    {

        private readonly ILogger _logger;
        public AlbumListQueryHandler(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<AlbumListResult> Handle(AlbumListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information("Requesting album list {@request}", request);
                var albums = GetAllAlbums(request.Skip, request.Take, request.SortBy, request.SortDirection, out int count);

                _logger.Information("Returning the requested page of albums from a total of {Count} albums", count);
                return await Task.FromResult(new AlbumListResult
                {
                    Albums = albums,
                    Total = count
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Errored while getting list of albums {Message}", ex.Message);
                return await Task.FromResult<AlbumListResult>(null);
            }
        }

        private List<AlbumModel> GetAllAlbums(int skip, int take, string sortBy, string sortDirection, out int count)
        {
            using (var context = AlbumsDbContext.Create())
            {
                var albums = (from alb in context.Albums
                              join albart in context.AlbumArtists on alb.Id equals albart.AlbumId
                              join art in context.Artists on albart.ArtistId equals art.Id
                              join inv in context.Inventory on alb.SKU equals inv.SKU
                              group art by new { alb, inv } into g
                              select new AlbumModel
                              {
                                  Id = g.Key.alb.Id,
                                  Name = g.Key.alb.AlbumName,
                                  SKU = g.Key.alb.SKU,
                                  Type = (Models.AlbumType)g.Key.alb.TypeId,
                                  Inventory = new InventoryModel
                                  {
                                      Id = g.Key.inv.Id,
                                      SKU = g.Key.inv.SKU,
                                      StockPurchased = g.Key.inv.StockPurchased,
                                      SoldSoFar = g.Key.inv.StockSoldSoFar,
                                      Stock = g.Key.inv.Stock
                                  },
                                  Artists = g.Select(t => new ArtistModel
                                  {
                                      Id = t.Id,
                                      Name = t.Name
                                  }).ToList()
                              });

                count = albums.Count();

                if (sortDirection.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                {
                    return albums
                    .OrderByDescending(BuildOrderByFunction(sortBy))
                    .Skip(skip)
                    .Take(take).ToList();
                }
                else
                {
                    return albums
                        .OrderBy(BuildOrderByFunction(sortBy))
                        .Skip(skip)
                        .Take(take).ToList();
                }
            }
        }

        private Func<AlbumModel, Object> BuildOrderByFunction(string sortBy)
        {
            Func<AlbumModel, Object> orderByFunction = null;
            switch (sortBy.ToLowerInvariant())
            {
                case "name":
                    orderByFunction = item => item.Name;
                    break;
                case "stock":
                    orderByFunction = item => item.Inventory.Stock;
                    break;
                case "artist":
                    orderByFunction = item => item.Artists.FirstOrDefault() == null || !item.Artists.Any() ? "" : item.Artists.FirstOrDefault().Name;
                    break;
                case "type":
                    orderByFunction = item => Enum.GetName(typeof(Models.AlbumType), item.Type);
                    break;
                default:
                    orderByFunction = item => item.Name;
                    break;
            }
            return orderByFunction;
        }

    }
}

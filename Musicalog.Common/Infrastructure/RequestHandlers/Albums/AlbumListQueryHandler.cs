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
                var albums = GetAllAlbums(request.Skip, request.Take, out int count);

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

        private List<AlbumModel> GetAllAlbums(int skip, int take, out int count)
        {
            using (var context = AlbumsDbContext.Create())
            {
                var albums = (from alb in context.Albums
                              join albart in context.AlbumArtists on alb.Id equals albart.AlbumId
                              join art in context.Artists on albart.ArtistId equals art.Id
                              join inv in context.Inventory on alb.SKU equals inv.SKU
                              group art by new { alb, inv } into g
                              select new
                              {
                                  Album = g.Key.alb,
                                  Artists = g.ToList(),
                                  Inventory = g.Key.inv
                              });


                count = albums.Count();

                return albums
                    .OrderBy(a => a.Album.Id)
                    .Skip(skip)
                    .Take(take)
                    .Select(a => new AlbumModel
                    {
                        Id = a.Album.Id,
                        Name = a.Album.AlbumName,
                        SKU = a.Album.SKU,
                        Type = (Models.AlbumType)a.Album.TypeId,
                        Inventory = new InventoryModel
                        {
                            Id = a.Inventory.Id,
                            SKU = a.Inventory.SKU,
                            StockPurchased = a.Inventory.StockPurchased,
                            SoldSoFar = a.Inventory.StockSoldSoFar,
                            Stock = a.Inventory.Stock
                        },
                        Artists = a.Artists.Select(t => new ArtistModel
                        {
                            Id = t.Id,
                            Name = t.Name
                        }).ToList()
                    }).ToList();
            }
        }

    }
}

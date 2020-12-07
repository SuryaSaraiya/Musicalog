using MediatR;
using Musicalog.Common.Data;
using Musicalog.Common.Data.Models;
using Musicalog.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Musicalog.Common.Infrastructure.RequestHandlers.Albums
{
    public class AlbumListQueryHandler : IRequestHandler<AlbumListQuery, AlbumListResult>
    {
        public async Task<AlbumListResult> Handle(AlbumListQuery request, CancellationToken cancellationToken)
        {
            var albums = GetAllAlbums(request.Skip, request.Take, out int count);

            return await Task.FromResult(new AlbumListResult
            {
                Albums = albums,
                Total = count
            });

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

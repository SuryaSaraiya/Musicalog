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
    public class AlbumQueryHandler : IRequestHandler<AlbumQuery, AlbumModel>
    {
        public async Task<AlbumModel> Handle(AlbumQuery request, CancellationToken cancellationToken)
        {
            var albums = QueryAlbumById(request.Id);

            return await Task.FromResult(albums);

        }

        private AlbumModel QueryAlbumById(int id)
        {
            using (var context = AlbumsDbContext.Create())
            {
                var albums = (from alb in context.Albums
                              join albart in context.AlbumArtists on alb.Id equals albart.AlbumId
                              join art in context.Artists on albart.ArtistId equals art.Id
                              where alb.Id == id
                              join inv in context.Inventory on alb.SKU equals inv.SKU
                              group art by new { alb, inv } into g
                              select new
                              {
                                  Album = g.Key.alb,
                                  Artists = g.ToList(),
                                  Inventory = g.Key.inv
                              });

                return albums.Select(a => new AlbumModel
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
                }).FirstOrDefault(); ;
            }
        }
    }
}

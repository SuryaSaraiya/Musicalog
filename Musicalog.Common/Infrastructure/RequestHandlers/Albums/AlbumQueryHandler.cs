﻿using MediatR;
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
    public class AlbumQueryHandler : IRequestHandler<AlbumQuery, AlbumModel>
    {
        private readonly ILogger _logger;

        public AlbumQueryHandler(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<AlbumModel> Handle(AlbumQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information("Requesting album details for {@request}", request);
                var album = QueryAlbumById(request.Id);

                _logger.Information("Returning requested album info as {@album}", album);
                return await Task.FromResult(album);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Errored getting album {id} with message {message}", request.Id, ex.Message);
                return await Task.FromResult<AlbumModel>(null);
            }
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

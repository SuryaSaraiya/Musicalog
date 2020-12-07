using MediatR;
using Musicalog.Common.Data;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Musicalog.Common.Infrastructure.RequestHandlers.Albums
{
    public class UpdateAlbumCommandHandler : IRequestHandler<UpdateAlbumCommand, int>
    {
        public async Task<int> Handle(UpdateAlbumCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using (var context = AlbumsDbContext.Create())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {

                        var album = context.Albums.SingleOrDefault(a => a.Id == request.AlbumId);

                        if (album != null)
                        {
                            var typeid = context.AlbumTypes.SingleOrDefault(t => t.Id == request.Type);

                            album.AlbumName = request.AlbumName;
                            album.TypeId = typeid.Id;

                            var inventory = context.Inventory.SingleOrDefault(i => i.SKU.Equals(album.SKU, StringComparison.InvariantCultureIgnoreCase));

                            if (inventory == null)
                            {
                                // insert inventory record
                                inventory = new Data.Models.Inventory
                                {
                                    SKU = album.SKU,
                                    Stock = request.Stock
                                };
                                context.Inventory.Add(inventory);
                            }
                            else
                            {
                                // update inventory record
                                inventory.Stock = request.Stock;
                            }

                            await context.SaveChangesAsync();

                        }

                        transaction.Commit();
                    }
                }

                return request.AlbumId;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}

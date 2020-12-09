using MediatR;
using Musicalog.Common.Data;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace Musicalog.Common.Infrastructure.RequestHandlers.Albums
{
    public class UpdateAlbumCommandHandler : IRequestHandler<UpdateAlbumCommand, bool>
    {

        private readonly ILogger _logger;

        public UpdateAlbumCommandHandler(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateAlbumCommand request, CancellationToken cancellationToken)
        {
            if (request.AlbumId <= 0
                || string.IsNullOrEmpty(request.AlbumName)
                || request.Stock < 0
                || request.Type <=0 
                || request.Artist == null || string.IsNullOrEmpty(request.Artist.Name))
            {
                _logger.Information("Requesting to update album with invalid request {@request}, returning false.", request);
                return false;
            }

            try
            {
                _logger.Information("Request to update album {@Request}", request);
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
                _logger.Information("Album was updated succesfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Errored while updating album with message {message}", ex.Message);
                return false;
            }
        }
    }
}

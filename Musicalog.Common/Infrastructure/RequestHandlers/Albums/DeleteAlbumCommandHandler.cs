using MediatR;
using Musicalog.Common.Data;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Musicalog.Common.Infrastructure.RequestHandlers.Albums
{
    public class DeleteAlbumCommandHandler : IRequestHandler<DeleteAlbumCommand, bool>
    {

        private readonly ILogger _logger;

        public DeleteAlbumCommandHandler(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteAlbumCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information("Request to delete album {@Request}", request);
                using (var context = AlbumsDbContext.Create())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        var album = context.Albums
                            .SingleOrDefault(a => a.Id == request.AlbumId);
                        if (album != null)
                        {

                            var albumArtists = context.AlbumArtists
                                .Where(a => a.AlbumId == request.AlbumId);
                            if (albumArtists != null && albumArtists.Any())
                            {
                                context.AlbumArtists.RemoveRange(albumArtists);
                                await context.SaveChangesAsync();
                            }

                            var albumInventory = context.Inventory
                                .Where(i => i.SKU.Equals(album.SKU, StringComparison.InvariantCultureIgnoreCase));
                            if (albumInventory != null && albumInventory.Any())
                            {
                                context.Inventory.RemoveRange(albumInventory);
                                await context.SaveChangesAsync();
                            }

                            context.Albums.Remove(album);
                            await context.SaveChangesAsync();

                            transaction.Commit();
                        }
                    }
                }

                _logger.Information("Album successfully deleted");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Errored trying to delete album with message {message}", ex.Message);
                return false;
            }
        }
    }
}

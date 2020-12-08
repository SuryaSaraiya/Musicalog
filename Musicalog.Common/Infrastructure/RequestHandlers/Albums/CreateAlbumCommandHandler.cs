using MediatR;
using Musicalog.Common.Data;
using Musicalog.Common.Data.Models;
using Musicalog.Common.Models;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Musicalog.Common.Infrastructure.RequestHandlers.Albums
{
    public class CreateAlbumCommandHandler : IRequestHandler<CreateAlbumCommand, int>
    {

        private static Random random = new Random();
        private readonly ILogger _logger;

        public CreateAlbumCommandHandler(ILogger logger)
        {
            _logger = logger;
        }


        public async Task<int> Handle(CreateAlbumCommand request, CancellationToken cancellationToken)
        {

            var albumId = 0;
            try
            {
                _logger.Information("Request for creating album {@request}", request);
                using (var context = AlbumsDbContext.Create())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        var typeid = context.AlbumTypes.SingleOrDefault(t => t.Id == request.Type);

                        var album = new Album
                        {
                            AlbumName = request.AlbumName,
                            TypeId = typeid.Id,
                            SKU = GenerateSku()
                        };

                        context.Albums.Add(album);
                        await context.SaveChangesAsync();
                        albumId = album.Id;

                        var artist = context.Artists.FirstOrDefault(a => a.Name.Replace(" ", "").Equals(request.ArtistName.Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase));

                        if (artist == null)
                        {
                            artist = new Artist
                            {
                                Name = request.ArtistName
                            };
                            context.Artists.Add(artist);
                            await context.SaveChangesAsync();
                        }

                        var albumArtist = new AlbumArtists
                        {
                            AlbumId = album.Id,
                            ArtistId = artist.Id
                        };

                        context.AlbumArtists.Add(albumArtist);

                        var inventory = new Inventory
                        {
                            SKU = album.SKU,
                            Stock = request.Stock
                        };

                        context.Inventory.Add(inventory);

                        await context.SaveChangesAsync();

                        transaction.Commit();
                    }
                }

                _logger.Information("Album was created with id {id}", albumId);
                return albumId;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Errored creating album with message {message}", ex.Message);
                return 0;
            }
        }

        private string GenerateSku()
        {
            return $"{GenerateRandonString(4)}-{GenerateRandonString(4)}";
        }
        private string GenerateRandonString(int len)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, len)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

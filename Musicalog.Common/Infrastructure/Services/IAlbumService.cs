using Musicalog.Common.Models;
using System.Threading.Tasks;

namespace Musicalog.Common.Infrastructure.Services
{
    public interface IAlbumService
    {
        Task<AlbumListResult> GetAllAlbums(int page_number, int page_size, string orderBy, string sortDirection);
        Task<AlbumModel> GetAlbum(int id);
        Task<bool> UpdateAlbum(AlbumModel album);
        Task<AlbumModel> CreateAlbum(AlbumModel album);
        Task<bool> DeleteAlbum(int id);
    }
}

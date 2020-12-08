﻿using MediatR;
using Musicalog.Common.Infrastructure.RequestHandlers.Albums;
using Musicalog.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Musicalog.Common.Infrastructure.Services
{

    public class AlbumService : IAlbumService
    {

        private readonly IMediator _mediator;

        public AlbumService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<AlbumModel> GetAlbum(int id)
        {
            var query = new AlbumQuery
            {
                Id = id
            };

            var album = await _mediator.Send(query);

            return album;
        }
        public async Task<AlbumListResult> GetAllAlbums(int page_number, int page_size)
        {
            var query = new AlbumListQuery
            {
                Id = 0,
                Name = string.Empty,
                Skip = (page_number - 1) * page_size,
                Take = page_size
            };

            var albumListResult = await _mediator.Send(query);

            return albumListResult;
        }
        public async Task<bool> UpdateAlbum(AlbumModel album)
        {
            var command = new UpdateAlbumCommand
            {
                AlbumId = album.Id,
                AlbumName = album.Name,
                Type = (int)album.Type,
                Stock = album.Inventory.Stock
            };

            var result = await _mediator.Send(command);

            return result == album.Id ? true : false;
        }
        public async Task<AlbumModel> CreateAlbum(AlbumModel album)
        {
            var command = new CreateAlbumCommand
            {
                AlbumId = 0,
                AlbumName = album.Name,
                ArtistName = album.Artists.FirstOrDefault()?.Name,
                Type = (int)album.Type,
                Stock = album.Inventory.Stock,
            };

            var result = await _mediator.Send(command);
            album.Id = result;
            return album;
        }
        public async Task<bool> DeleteAlbum(int id)
        {
            var command = new DeleteAlbumCommand
            {
                AlbumId = id
            };

            var result = await _mediator.Send(command);

            return result;
        }

    }
}

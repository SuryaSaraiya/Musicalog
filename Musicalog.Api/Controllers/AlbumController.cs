﻿using Musicalog.Common.Infrastructure.Services;
using Musicalog.Common.Models;
using System.Threading.Tasks;
using System.Web.Http;

namespace Musicalog.Api.Controllers
{
    [RoutePrefix("api/Album")]
    public class AlbumController : ApiController
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetAlbumById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var album = await _albumService.GetAlbum(id);

            if (album == null)
            {
                return NotFound();
            }

            return Ok(album);
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IHttpActionResult> UpdateAlbumById(AlbumModel album)
        {
            if (album.Id <= 0)
            {
                return BadRequest();
            }

            await _albumService.UpdateAlbum(album);

            if (album == null)
            {
                return NotFound();
            }

            return Ok(album);
        }

        [HttpPut]
        public async Task<IHttpActionResult> CreateAlbum(AlbumModel album)
        {
            album = await _albumService.CreateAlbum(album);

            return Ok(album);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> DelteAlbum(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var result = await _albumService.DeleteAlbum(id);

            return Ok(result);
        }

        [HttpGet]
        [Route("list")]
        public async Task<IHttpActionResult> GetAllAlbums(int page = 1, int pageSize = 10)
        {
            var result = await _albumService.GetAllAlbums(page, pageSize);

            if (result == null && result.Albums != null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}

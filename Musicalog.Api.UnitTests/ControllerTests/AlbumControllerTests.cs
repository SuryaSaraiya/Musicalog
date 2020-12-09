using System.Web.Http;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Musicalog.Api.Controllers;
using Musicalog.Common.Infrastructure.Services;
using Musicalog.Common.Models;
using System;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using System.Web.Http.Results;
using System.Collections.Generic;

namespace Musicalog.Api.UnitTests
{
    public class AlbumControllerTests : TestBase<AlbumController>
    {
        public AlbumControllerTests()
        {
            InitializeObjectUnderTest();
        }

        [Fact]
        public async void Get_Album_ById_Non_0_Returns_Album()
        {
            var fix = new Fixture();
            var album = fix.Create<AlbumModel>();
            var nonZeroAlbumId = 2;

            For<IAlbumService>()
                .Setup(s => s.GetAlbum(It.IsAny<int>()))
                .Returns(Task.FromResult(album)).Verifiable();

            IHttpActionResult model = await ObjectUnderTest.GetAlbumById(nonZeroAlbumId);

            model.ShouldNotBeNull();
            model.ShouldNotBeOfType<BadRequestResult>();
            model.ShouldNotBeOfType<NotFoundResult>();
            model.ShouldBeOfType<OkNegotiatedContentResult<AlbumModel>>();
            For<IAlbumService>().Verify();
        }

        [Fact]
        public async void Get_Album_ById_0_Returns_Bad_Request()
        {
            var zeroAlbumId = 0;

            IHttpActionResult model = await ObjectUnderTest.GetAlbumById(zeroAlbumId);

            model.ShouldNotBeNull();
            model.ShouldBeOfType<BadRequestResult>();
            model.ShouldNotBeOfType<OkResult>();
        }

        [Fact]
        public async void Get_Album_ById_Non_0_Not_Found_Returns_Not_Found()
        {
            var nonZeroAlbumId = 2;

            For<IAlbumService>()
                .Setup(s => s.GetAlbum(It.IsAny<int>()))
                .Returns(Task.FromResult<AlbumModel>(null)).Verifiable();

            IHttpActionResult model = await ObjectUnderTest.GetAlbumById(nonZeroAlbumId);

            model.ShouldNotBeNull();
            model.ShouldNotBeOfType<BadRequestResult>();
            model.ShouldNotBeOfType<OkNegotiatedContentResult<AlbumModel>>();
            model.ShouldBeOfType<NotFoundResult>();
            For<IAlbumService>().Verify();
        }

        [Fact]
        public async void Update_Album_ById_Non_0_Returns_Album()
        {
            var fix = new Fixture();
            var album = fix.Create<AlbumModel>();
            var nonZeroAlbumId = 2;
            album.Id = nonZeroAlbumId;

            For<IAlbumService>()
                .Setup(s => s.UpdateAlbum(It.IsAny<AlbumModel>()))
                .Returns(Task.FromResult(true)).Verifiable();

            IHttpActionResult result = await ObjectUnderTest.UpdateAlbum(album);

            var albumId = result as OkNegotiatedContentResult<AlbumModel>;

            result.ShouldNotBeNull();
            result.ShouldNotBeOfType<BadRequestResult>();
            result.ShouldNotBeOfType<NotFoundResult>();
            result.ShouldBeOfType<OkNegotiatedContentResult<AlbumModel>>();
            albumId.Content.Id.ShouldBe(album.Id);
            For<IAlbumService>().Verify();
        }

        [Fact]
        public async void Update_Album_ById_0_Returns_Bad_Request()
        {
            var fix = new Fixture();
            var album = fix.Create<AlbumModel>();
            album.Id = 0;

            IHttpActionResult model = await ObjectUnderTest.UpdateAlbum(album);

            model.ShouldNotBeNull();
            model.ShouldNotBeOfType<OkNegotiatedContentResult<AlbumModel>>();
            model.ShouldBeOfType<BadRequestResult>();
        }

        [Fact]
        public async void Update_Album_ById_Non_0_ISE_Returns_ISE()
        {
            var fix = new Fixture();
            var album = fix.Create<AlbumModel>();
            var nonZeroAlbumId = 2;
            album.Id = nonZeroAlbumId;

            For<IAlbumService>()
                .Setup(s => s.UpdateAlbum(It.IsAny<AlbumModel>()))
                .Returns(Task.FromResult(false)).Verifiable();

            IHttpActionResult result = await ObjectUnderTest.UpdateAlbum(album);

            result.ShouldNotBeNull();
            result.ShouldNotBeOfType<BadRequestResult>();
            result.ShouldNotBeOfType<OkNegotiatedContentResult<AlbumModel>>();
            result.ShouldBeOfType<ExceptionResult>();
            For<IAlbumService>().Verify();
        }

        [Fact]
        public async void Create_Album_ById_0_Returns_Album()
        {
            var fix = new Fixture();
            var inputAlbum = fix.Create<AlbumModel>();
            var zeroAlbumId = 0;
            inputAlbum.Id = zeroAlbumId;

            var outputAlbum = fix.Create<AlbumModel>();
            outputAlbum.Id = 4;

            For<IAlbumService>()
                .Setup(s => s.CreateAlbum(It.IsAny<AlbumModel>()))
                .Returns(Task.FromResult(outputAlbum)).Verifiable();

            IHttpActionResult result = await ObjectUnderTest.CreateAlbum(inputAlbum);

            var albumId = result as OkNegotiatedContentResult<AlbumModel>;

            result.ShouldNotBeNull();
            result.ShouldNotBeOfType<BadRequestResult>();
            result.ShouldNotBeOfType<NotFoundResult>();
            result.ShouldBeOfType<OkNegotiatedContentResult<AlbumModel>>();
            albumId.Content.Id.ShouldBe(outputAlbum.Id);
            For<IAlbumService>().Verify();
        }

        [Fact]
        public async void Create_Album_ById_Non_0_Returns_Bad_Request()
        {
            var fix = new Fixture();
            var album = fix.Create<AlbumModel>();
            album.Id = 2;

            IHttpActionResult model = await ObjectUnderTest.CreateAlbum(album);

            model.ShouldNotBeNull();
            model.ShouldNotBeOfType<OkNegotiatedContentResult<AlbumModel>>();
            model.ShouldBeOfType<BadRequestResult>();
        }

        [Fact]
        public async void Create_Album_ById_0_When_NULL_Returns_ISE()
        {
            var fix = new Fixture();
            var album = fix.Create<AlbumModel>();
            var zeroAlbumId = 0;
            album.Id = zeroAlbumId;

            For<IAlbumService>()
                .Setup(s => s.CreateAlbum(It.IsAny<AlbumModel>()))
                .Returns(Task.FromResult<AlbumModel>(null)).Verifiable();

            IHttpActionResult result = await ObjectUnderTest.CreateAlbum(album);

            result.ShouldNotBeNull();
            result.ShouldNotBeOfType<BadRequestResult>();
            result.ShouldNotBeOfType<OkNegotiatedContentResult<AlbumModel>>();
            result.ShouldBeOfType<ExceptionResult>();
            For<IAlbumService>().Verify();
        }

        [Fact]
        public async void Delete_Album_ById_0_Returns_Bad_Request()
        {
            var zeroAlbumId = 0;
            IHttpActionResult result = await ObjectUnderTest.DelteAlbum(zeroAlbumId);

            result.ShouldNotBeNull();
            result.ShouldNotBeOfType<OkNegotiatedContentResult<bool>>();
            result.ShouldBeOfType<BadRequestResult>();
        }

        [Fact]
        public async void Delete_Album_ById_Non_0_Deletes_Returns_True()
        {
            var fix = new Fixture();
            var album = fix.Create<AlbumModel>();
            var zeroAlbumId = 4;
            album.Id = zeroAlbumId;

            For<IAlbumService>()
                .Setup(s => s.DeleteAlbum(It.IsAny<int>()))
                .Returns(Task.FromResult<bool>(true)).Verifiable();

            IHttpActionResult result = await ObjectUnderTest.DelteAlbum(album.Id);

            var response = result as OkNegotiatedContentResult<bool>;

            result.ShouldNotBeNull();
            result.ShouldNotBeOfType<BadRequestResult>();
            result.ShouldBeOfType<OkNegotiatedContentResult<bool>>();
            response.Content.ShouldBeTrue();
            For<IAlbumService>().Verify();
        }

        [Fact]
        public async void Delete_Album_ById_Non_0_Does_Not_Delete_Returns_False()
        {
            var fix = new Fixture();
            var album = fix.Create<AlbumModel>();
            var zeroAlbumId = 4;
            album.Id = zeroAlbumId;

            For<IAlbumService>()
                .Setup(s => s.DeleteAlbum(It.IsAny<int>()))
                .Returns(Task.FromResult<bool>(false)).Verifiable();

            IHttpActionResult result = await ObjectUnderTest.DelteAlbum(album.Id);

            var response = result as OkNegotiatedContentResult<bool>;

            result.ShouldNotBeNull();
            result.ShouldNotBeOfType<BadRequestResult>();
            result.ShouldBeOfType<OkNegotiatedContentResult<bool>>();
            response.Content.ShouldBeFalse();
            For<IAlbumService>().Verify();
        }


        [Fact]
        public async void Get_Albums_When_Found_Returns_Results()
        {
            var fix = new Fixture();
            var expectedResponse = fix.Create<AlbumListResult>();
            expectedResponse.Total = 20;
            expectedResponse.Albums = fix.Create<List<AlbumModel>>();

            For<IAlbumService>()
                .Setup(s => s.GetAllAlbums(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<AlbumListResult>(expectedResponse)).Verifiable();

            IHttpActionResult result = await ObjectUnderTest.GetAllAlbums(1, 10);

            var response = result as OkNegotiatedContentResult<AlbumListResult>;

            result.ShouldNotBeNull();
            result.ShouldBeOfType<OkNegotiatedContentResult<AlbumListResult>>();
            response.Content.Total.ShouldBe(expectedResponse.Total);
            response.Content.Albums.ShouldBeEquivalentTo(expectedResponse.Albums);
            For<IAlbumService>().Verify();
        }

        [Fact]
        public async void Get_Albums_When_None_Found_Returns_Not_Found()
        {
            var fix = new Fixture();
            var expectedResponse = fix.Create<AlbumListResult>();
            expectedResponse.Total = 0;
            expectedResponse.Albums = null;

            For<IAlbumService>()
                .Setup(s => s.GetAllAlbums(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<AlbumListResult>(expectedResponse)).Verifiable();

            IHttpActionResult result = await ObjectUnderTest.GetAllAlbums(1, 10);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<NotFoundResult>();
            For<IAlbumService>().Verify();
        }
    }
}

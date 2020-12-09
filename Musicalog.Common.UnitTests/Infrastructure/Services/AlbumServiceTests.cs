using AutoFixture;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Musicalog.Common.Infrastructure.RequestHandlers.Albums;
using Musicalog.Common.Infrastructure.Services;
using Musicalog.Common.Models;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Musicalog.Common.UnitTests.Infrastructure.Services
{
    public class AlbumServiceTests : TestBase<AlbumService>
    {
        public AlbumServiceTests()
        {
            InitializeObjectUnderTest();
        }

        [Fact]
        public async void GetAllAlbums_Returns_Valid_List_Of_Albums()
        {
            var pageNumber = 1;
            var pageSize = 10;
            var sortBy = "name";
            var sortDirection = "asc";

            var fix = new Fixture();
            var albumListResult = fix.Create<AlbumListResult>();

            For<IMediator>()
                .Setup(m => m.Send(It.IsAny<AlbumListQuery>(), default(CancellationToken)))
                .Returns(Task.FromResult(albumListResult)).Verifiable();

            var result = await ObjectUnderTest.GetAllAlbums(pageNumber, pageSize, sortBy, sortDirection);

            result.ShouldNotBeNull();
            result.ShouldBeEquivalentTo(albumListResult);
            For<IMediator>().Verify();
        }

        [Fact]
        public async void GetAlbum_ById_Non_0_Returns_Valid_List_Of_Albums()
        {
            var albumId = 1;

            var fix = new Fixture();
            var album = fix.Create<AlbumModel>();

            For<IMediator>()
                .Setup(m => m.Send(It.IsAny<AlbumQuery>(), default(CancellationToken)))
                .Returns(Task.FromResult(album)).Verifiable();

            var result = await ObjectUnderTest.GetAlbum(albumId);

            result.ShouldNotBeNull();
            result.ShouldBeEquivalentTo(album);
            For<IMediator>().Verify();
        }

        [Fact]
        public async void GetAlbum_ById_0_Returns_Null()
        {
            var albumId = 0;

            For<IMediator>()
                .Setup(m => m.Send(It.IsAny<AlbumQuery>(), default(CancellationToken)))
                .Returns(Task.FromResult<AlbumModel>(null)).Verifiable();

            var result = await ObjectUnderTest.GetAlbum(albumId);

            result.ShouldBeNull();
            For<IMediator>().Verify();
        }

        [Fact]
        public async void UpdateAlbum_Valid_Request_Returns_True()
        {
            var fix = new Fixture();
            var validAlbum = fix.Create<AlbumModel>();

            For<IMediator>()
                .Setup(m => m.Send(It.IsAny<UpdateAlbumCommand>(), default(CancellationToken)))
                .Returns(Task.FromResult(true)).Verifiable();

            var result = await ObjectUnderTest.UpdateAlbum(validAlbum);

            result.ShouldBeTrue();
            For<IMediator>().Verify();
        }

        [Fact]
        public async void UpdateAlbum_InValid_Request_Returns_False()
        {
            var fix = new Fixture();
            var invalidAlbum = fix.Create<AlbumModel>();

            For<IMediator>()
                .Setup(m => m.Send(It.IsAny<UpdateAlbumCommand>(), default(CancellationToken)))
                .Returns(Task.FromResult(false)).Verifiable();

            var result = await ObjectUnderTest.UpdateAlbum(invalidAlbum);

            result.ShouldBeFalse();
            For<IMediator>().Verify();
        }

        [Fact]
        public async void CreateAlbum_Valid_Request_Returns_True()
        {
            var newAlbumId = 12;
            var zero = 0;
            var fix = new Fixture();
            var validAlbum = fix.Create<AlbumModel>();
            validAlbum.Id = zero;

            For<IMediator>()
                .Setup(m => m.Send(It.IsAny<CreateAlbumCommand>(), default(CancellationToken)))
                .Returns(Task.FromResult(newAlbumId)).Verifiable();

            var result = await ObjectUnderTest.CreateAlbum(validAlbum);

            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(zero);
            result.Id.ShouldBe(newAlbumId);
            For<IMediator>().Verify();
        }

        [Fact]
        public async void CreateAlbum_InValid_Request_Returns_False()
        {
            var zero = 0;
            var fix = new Fixture();
            var invalidAlbum = fix.Create<AlbumModel>();
            invalidAlbum.Id = zero;

            For<IMediator>()
                .Setup(m => m.Send(It.IsAny<CreateAlbumCommand>(), default(CancellationToken)))
                .Returns(Task.FromResult(zero)).Verifiable();

            var result = await ObjectUnderTest.CreateAlbum(invalidAlbum);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(zero);
            For<IMediator>().Verify();
        }
    }
}

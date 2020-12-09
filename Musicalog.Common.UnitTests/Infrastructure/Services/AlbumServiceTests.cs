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
    }
}

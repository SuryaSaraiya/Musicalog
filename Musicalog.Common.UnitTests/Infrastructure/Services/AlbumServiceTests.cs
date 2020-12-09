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
        public async void When_GetAlbums_Has_Valid_Parameters_Then_Returns_Valid_List()
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
    }
}

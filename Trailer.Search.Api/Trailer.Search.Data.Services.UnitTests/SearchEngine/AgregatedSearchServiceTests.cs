using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trailer.Search.Data.Services.SearchEngine;
using Trailer.Search.Data.Services.SearchResults;
using Trailer.Search.Data.Services.Tmdb;
using Trailer.Search.Data.Services.Youtube;
using FluentAssertions;
namespace Trailer.Search.Data.Services.UnitTests.SearchEngine
{
    [TestFixture]
    public class AgregatedSearchServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IMovieDatabaseSearch> mockMovieDatabaseSearch;
        private Mock<IVideoServiceSearch> mockVideoServiceSearch;
        private List<TrailerSearchResult> mockResultSet;

        [SetUp]
        public void SetUp()
        {
            mockResultSet = new List<TrailerSearchResult>();

            mockResultSet.Add(new TrailerSearchResult()
            {
                ThumbnailUrl = "turl",
                Title = "test1resultmv trailer",
                Uid = "uidmovie1",
                Url = "someurl1m"

            });


            mockResultSet.Add(new TrailerSearchResult()
            {
                ThumbnailUrl = "turl",
                Title = "test2resultmv+vd trailer",
                Uid = "uidmovie2",
                Url = "someurl2m"

            });
            mockResultSet.Add(new TrailerSearchResult()
            {
                ThumbnailUrl = "turl",
                Title = "test2resultmv+vd trailer",
                Uid = "uidvideo2",
                Url = "someurl2v"

            });

            mockResultSet.Add(new TrailerSearchResult()
            {
                ThumbnailUrl = "turl",
                Title = "test3resultvd trailer",
                Uid = "uidvideo3",
                Url = "someurl3v"

            });


            //duplicate results  setup 
            mockResultSet.Add(new TrailerSearchResult()
            {
                ThumbnailUrl = "turl",
                Title = "test4resultmv+vd trailer",
                Uid = "uidmovie4",
                Url = "someurl4"

            });
            mockResultSet.Add(new TrailerSearchResult()
            {
                ThumbnailUrl = "turl",
                Title = "test4resultmv+vd trailer",
                Uid = "uidvideo4",
                Url = "someurl4"

            });






            this.mockRepository = new MockRepository(MockBehavior.Loose);

            this.mockMovieDatabaseSearch = this.mockRepository.Create<IMovieDatabaseSearch>();
            this.mockVideoServiceSearch = this.mockRepository.Create<IVideoServiceSearch>();


            this.mockMovieDatabaseSearch.Setup(x => x.Search(string.Empty)).ReturnsAsync(new List<TrailerSearchResult>());
            this.mockVideoServiceSearch.Setup(x => x.Search(string.Empty)).ReturnsAsync(new List<TrailerSearchResult>());


            this.mockMovieDatabaseSearch.Setup(x => x.Search("test1resultmv")).ReturnsAsync(mockResultSet.Where(x => x.Title == "test1resultmv trailer" && x.Uid.Contains("movie")));
            this.mockVideoServiceSearch.Setup(x => x.Search("test1resultmv")).ReturnsAsync(mockResultSet.Where(x => x.Title == "test1resultmv trailer" && x.Uid.Contains("video")));


            this.mockMovieDatabaseSearch.Setup(x => x.Search("test2resultmv+vd")).ReturnsAsync(mockResultSet.Where(x => x.Title == "test2resultmv+vd trailer" && x.Uid.Contains("movie")));
            this.mockVideoServiceSearch.Setup(x => x.Search("test2resultmv+vd")).ReturnsAsync(mockResultSet.Where(x => x.Title == "test2resultmv+vd trailer" && x.Uid.Contains("video")));


            this.mockMovieDatabaseSearch.Setup(x => x.Search("test3resultvd")).ReturnsAsync(mockResultSet.Where(x => x.Title == "test3resultvd trailer" && x.Uid.Contains("movie")));
            this.mockVideoServiceSearch.Setup(x => x.Search("test3resultvd")).ReturnsAsync(mockResultSet.Where(x => x.Title == "test3resultvd trailer" && x.Uid.Contains("video")));


            this.mockMovieDatabaseSearch.Setup(x => x.Search("test4resultmv+vd")).ReturnsAsync(mockResultSet.Where(x => x.Title == "test4resultmv+vd trailer" && x.Uid.Contains("movie")));
            this.mockVideoServiceSearch.Setup(x => x.Search("test4resultmv+vd")).ReturnsAsync(mockResultSet.Where(x => x.Title == "test4resultmv+vd trailer" && x.Uid.Contains("video")));
        }

        [TearDown]
        public void TearDown()
        {

        }

        private AgregatedSearchService CreateService()
        {
            return new AgregatedSearchService(
                this.mockMovieDatabaseSearch.Object,
                this.mockVideoServiceSearch.Object);
        }

        [Test]
        public async Task Search_NoResult_ReturnsEmptyList()
        {
            // Arrange
            var service = this.CreateService();
            string movieName = String.Empty;

            // Act
            var result = await service.Search(
                movieName);

            // Assert
            result.Should().BeEmpty();
        }
        [Test]
        public async Task Search_ResultsOnlyFromMovieDatabase_Returns1Result()
        {
            // Arrange
            var service = this.CreateService();
            string movieName = "test1resultmv";

            // Act
            var result = await service.Search(
                movieName);

            // Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);

        }

        [Test]
        public async Task Search_ResultsFromMovieDatabaseAndVideoService_Returns2Results()
        {
            // Arrange
            var service = this.CreateService();
            string movieName = "test2resultmv+vd";

            // Act
            var result = await service.Search(
                movieName);

            // Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(2);

        }

        [Test]
        public async Task Search_ResultsOnlyFromVideoService_Returns1Result()
        {
            // Arrange
            var service = this.CreateService();
            string movieName = "test3resultvd";

            // Act
            var result = await service.Search(
                movieName);

            // Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);

        }
        [Test]
        public async Task Search_ResultsFromVideoServiceAlreadyPresentInMovieDbRTesults_Returns1Result()
        {
            // Arrange
            var service = this.CreateService();
            string movieName = "test4resultmv+vd";

            // Act
            var result = await service.Search(
                movieName);

            // Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);

        }

    }
}

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trailer.Search.Api.Controllers;
using Trailer.Search.Data.Services;
using Trailer.Search.Data.Services.SearchResults;

namespace Trailer.Search.Api.UnitTests.Controllers
{
    [TestFixture]
    public class SearchTrailersControllerTests
    {
        private MockRepository mockRepository;

        private Mock<ISearchService> mockSearchService;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Loose);

            this.mockSearchService = this.mockRepository.Create<ISearchService>();

        }

        [TearDown]
        public void TearDown()
        {

        }

        private SearchTrailersController CreateSearchTrailersController()
        {
            return new SearchTrailersController(
                this.mockSearchService.Object);
        }

        [Test]
        public async Task Get_QueryIsEmpty_ReturnsBadRquest()
        {
            // Arrange
            var searchTrailersController = this.CreateSearchTrailersController();
            string query = string.Empty;

            this.mockSearchService.Setup(x => x.Search(It.IsAny<string>())).ReturnsAsync(new List<TrailerSearchResult>());
            // Act
            var result = await searchTrailersController.Get(
                query);

            // Assert
            result.Result.Should().BeOfType(typeof(BadRequestResult));
        }

        [Test]
        public async Task Get_QueryFindsNoResults_ReturnsNotFound()
        {
            // Arrange
            var searchTrailersController = this.CreateSearchTrailersController();
            string query = "can't touch this :)";

            this.mockSearchService.Setup(x => x.Search(It.IsAny<string>())).ReturnsAsync(new List<TrailerSearchResult>());
            // Act
            var result = await searchTrailersController.Get(
                query);

            // Assert
            result.Result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Test]
        public async Task Get_QueryFindsResults_ReturnsOkResult()
        {
            // Arrange
            var searchTrailersController = this.CreateSearchTrailersController();
            string query = "deadpool";


            this.mockSearchService.Setup(
                x => x.Search(
                    It.IsAny<string>()))
                .ReturnsAsync(
                    new List<TrailerSearchResult>() {
                        new TrailerSearchResult() {
                            ThumbnailUrl = "deadpool's Thumb",
                            Title = "Deadpool trailer",
                            Uid = "unique deadpool" ,
                            Url = "deadpool's url "
                        } });

            // Act
            var result = await searchTrailersController.Get(
                query);

            // Assert
            result.Result.Should().BeOfType(typeof(OkObjectResult));
        }
        [Test]
        public async Task Get_QueryFindsResults_ReturnsOkResultWithcorrectNumberOfItems()
        {
            // Arrange
            var searchTrailersController = this.CreateSearchTrailersController();
            string query = "deadpool";


            this.mockSearchService.Setup(
                x => x.Search(
                    It.IsAny<string>()))
                .ReturnsAsync(
                    new List<TrailerSearchResult>() {
                        new TrailerSearchResult() {
                            ThumbnailUrl = "deadpool's Thumb",
                            Title = "Deadpool trailer",
                            Uid = "unique deadpool" ,
                            Url = "deadpool's url "
                        } });

            // Act
            var result = await searchTrailersController.Get(
                query);           
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            var castResult = (OkObjectResult)result.Result;
            var responseValue = (IEnumerable<TrailerSearchResult>)castResult.Value;
            // Assert
            responseValue.Should().HaveCount(1);

        }

    }
}

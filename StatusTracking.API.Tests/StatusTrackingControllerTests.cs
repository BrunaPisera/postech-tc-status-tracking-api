using Microsoft.AspNetCore.Mvc;
using Moq;
using StatusTracking.API.Controllers;
using StatusTracking.Core.Entities;
using StatusTracking.Core.Entities.Enums;
using StatusTracking.UseCases.Interfaces;

namespace StatusTracking.API.Tests
{
    public class StatusTrackingControllerTests
    {
        private Mock<IStatusTrackingUseCases> _mockUseCases;
        private StatusTrackingController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockUseCases = new Mock<IStatusTrackingUseCases>();
            _controller = new StatusTrackingController(_mockUseCases.Object);
        }

        [Test]
        public async Task Get_ReturnsOk_WhenVideoExists()
        {
            var videoId = "123";
            var expectedVideo = new StatusTrackingAggregate
            {
                VideoId = videoId,
                Status = Status.InProcess
            };

            _mockUseCases.Setup(x => x.GetVideoByIdAsync(videoId))
                            .ReturnsAsync(expectedVideo);

            var result = await _controller.Get(videoId);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.TypeOf<StatusTrackingAggregate>());

            var returnedVideo = (StatusTrackingAggregate)okResult.Value;
            Assert.That(returnedVideo.VideoId, Is.EqualTo(expectedVideo.VideoId));
            Assert.That(returnedVideo.Status, Is.EqualTo(expectedVideo.Status));
        }

        [Test]
        public async Task Get_ReturnsNotFound_WhenVideoIsNull()
        {
            var videoId = "not-found";
            _mockUseCases.Setup(x => x.GetVideoByIdAsync(videoId))
                            .ReturnsAsync((StatusTrackingAggregate)null);

            var result = await _controller.Get(videoId);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Get_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            var videoId = "error";
            _mockUseCases.Setup(x => x.GetVideoByIdAsync(videoId))
                            .ThrowsAsync(new Exception("Something went wrong"));

            var result = await _controller.Get(videoId);

            var badRequest = result as BadRequestObjectResult;
            Assert.That(badRequest, Is.Not.Null);
            Assert.That(badRequest.Value, Is.EqualTo("Something went wrong"));
        }
    }
}
using Moq;
using StatusTracking.Core.Entities;
using StatusTracking.Core.Entities.Enums;
using StatusTracking.UseCases.DTOs;
using StatusTracking.UseCases.Exceptions;
using StatusTracking.UseCases.Gateway;

namespace StatusTracking.UseCases.Tests
{
    public class StatusTrackingUseCasesTests
    {
        private Mock<IStatusTrackingPersistenceGateway> _mockGateway;
        private StatusTrackingUseCases _useCases;

        [SetUp]
        public void SetUp()
        {
            _mockGateway = new Mock<IStatusTrackingPersistenceGateway>();
            _useCases = new StatusTrackingUseCases(_mockGateway.Object);
        }

        [Test]
        public async Task AtualizaStatusComoInProcessAsync_UpdatesStatus_WhenStatusIsUploaded()
        {
            var videoId = "123";
            var video = new StatusTrackingAggregate { VideoId = videoId, Status = Status.Uploaded };

            _mockGateway.Setup(x => x.GetVideoByIdAsync(videoId))
                        .ReturnsAsync(video);
            _mockGateway.Setup(x => x.SaveStatusAsync(It.IsAny<StatusTrackingAggregate>()))
                        .ReturnsAsync(true);

            await _useCases.AtualizaStatusComoInProcessAsync(videoId);

            Assert.That(video.Status, Is.EqualTo(Status.InProcess));
        }

        [Test]
        public void AtualizaStatusComoInProcessAsync_Throws_WhenStatusIsNotUploaded()
        {
            var videoId = "123";
            var video = new StatusTrackingAggregate { VideoId = videoId, Status = Status.Ready };

            _mockGateway.Setup(x => x.GetVideoByIdAsync(videoId))
                        .ReturnsAsync(video);

            var ex = Assert.ThrowsAsync<OperacaoInvalidaException>(() =>
                _useCases.AtualizaStatusComoInProcessAsync(videoId));

            Assert.That(ex.Message, Does.Contain("precisa estar como uploaded"));
        }

        [Test]
        public async Task AtualizaStatusComoReadyAsync_UpdatesStatusAndUrl_WhenStatusIsInProcess()
        {
            var videoId = "123";
            var dto = new VideoProcessedMessageDto { VideoKey = videoId, FilesURL = "https://url.com/zip" };
            var video = new StatusTrackingAggregate { VideoId = videoId, Status = Status.InProcess };

            _mockGateway.Setup(x => x.GetVideoByIdAsync(videoId))
                        .ReturnsAsync(video);
            _mockGateway.Setup(x => x.SaveStatusAsync(It.IsAny<StatusTrackingAggregate>()))
                        .ReturnsAsync(true);

            await _useCases.AtualizaStatusComoReadyAsync(dto);

            Assert.That(video.Status, Is.EqualTo(Status.Ready));
            Assert.That(video.VideoImagesZipFileUrl, Is.EqualTo(dto.FilesURL));
        }

        [Test]
        public void AtualizaStatusComoReadyAsync_Throws_WhenStatusIsNotInProcess()
        {
            var videoId = "123";
            var dto = new VideoProcessedMessageDto { VideoKey = videoId, FilesURL = "https://url.com/zip" };
            var video = new StatusTrackingAggregate { VideoId = videoId, Status = Status.Uploaded };

            _mockGateway.Setup(x => x.GetVideoByIdAsync(videoId))
                        .ReturnsAsync(video);

            var ex = Assert.ThrowsAsync<OperacaoInvalidaException>(() =>
                _useCases.AtualizaStatusComoReadyAsync(dto));

            Assert.That(ex.Message, Does.Contain("precisa estar como InProcess"));
        }

        [Test]
        public async Task SaveUploadedVideoAsync_SavesVideoWithUploadedStatus()
        {
            var videoId = "456";
            StatusTrackingAggregate? savedVideo = null;

            _mockGateway.Setup(x => x.SaveStatusAsync(It.IsAny<StatusTrackingAggregate>()))
                        .Callback<StatusTrackingAggregate>(v => savedVideo = v)
                        .ReturnsAsync(true);

            await _useCases.SaveUploadedVideoAsync(videoId);

            Assert.That(savedVideo, Is.Not.Null);
            Assert.That(savedVideo!.VideoId, Is.EqualTo(videoId));
            Assert.That(savedVideo.Status, Is.EqualTo(Status.Uploaded));
        }

        [Test]
        public async Task AtualizaStatusComoErrorAsync_SavesVideoWithErrorStatus()
        {
            var videoId = "789";
            StatusTrackingAggregate? savedVideo = null;

            _mockGateway.Setup(x => x.SaveStatusAsync(It.IsAny<StatusTrackingAggregate>()))
                        .Callback<StatusTrackingAggregate>(v => savedVideo = v)
                        .ReturnsAsync(true);

            await _useCases.AtualizaStatusComoErrorAsync(videoId);

            Assert.That(savedVideo, Is.Not.Null);
            Assert.That(savedVideo!.VideoId, Is.EqualTo(videoId));
            Assert.That(savedVideo.Status, Is.EqualTo(Status.Error));
            Assert.That(savedVideo.Errors, Does.Contain("Could not process"));
        }

        [Test]
        public async Task GetVideoByIdAsync_ReturnsVideo()
        {
            var videoId = "xyz";
            var expected = new StatusTrackingAggregate { VideoId = videoId };

            _mockGateway.Setup(x => x.GetVideoByIdAsync(videoId))
                        .ReturnsAsync(expected);

            var result = await _useCases.GetVideoByIdAsync(videoId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.VideoId, Is.EqualTo(videoId));
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using StatusTracking.UseCases.Interfaces;

namespace StatusTracking.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusTrackingController : ControllerBase
    {
        private readonly IStatusTrackingUseCases _statusTrackingUseCases;
        public StatusTrackingController(IStatusTrackingUseCases statusTrackingUseCases) 
        {
            _statusTrackingUseCases = statusTrackingUseCases;
        }
        [HttpGet("{videoId}")]
        public async Task<IActionResult> Get([FromRoute] string videoId)
        {
            try
            {
                var video = await _statusTrackingUseCases.GetVideoByIdAsync(videoId);

                if (video == null) return NotFound();

                return Ok(video);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

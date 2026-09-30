using IOT.DBContext;
using IOT.DTOs;
using IOT.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IOT.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SoilReadingsController : Controller
    {
        private readonly ISoilReadingRepository _soilReadingRepository;

        public SoilReadingsController(ISoilReadingRepository soilReadingRepository)
        {
            _soilReadingRepository = soilReadingRepository;
        }

        [HttpPost]
        [Route("postsolireading")]
        public async Task<IActionResult> PostSoliReading([FromBody] MultiSensorPayloadDto multiSensorPayloadDto)
        {
            if (multiSensorPayloadDto == null)
                return BadRequest("Invalid data.");

            var res = await _soilReadingRepository.AddSoilReadingAsync(multiSensorPayloadDto);

            if(res)
            {
                return Ok(new { message = "Data saved successfully", success = res });
            }
            else
            {
                return StatusCode(500, new { message = "An error occurred while saving the data.", success = res });
            }
        }
    }
}

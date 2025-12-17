using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoBooker.Application.DTOs.Photographers;
using PhotoBooker.Application.Interfaces;

namespace PhotoBooker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PhotographersController : ControllerBase
{
    private readonly IPhotographerService _photographerService;
    private readonly ILogger<PhotographersController> _logger;

    public PhotographersController(
        IPhotographerService photographerService,
        ILogger<PhotographersController> logger)
    {
        _photographerService = photographerService;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<PhotographerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PhotographerDto>>> GetPhotographers()
    {
        try
        {
            _logger.LogInformation("Fetching all photographers");
            var photographers = await _photographerService.GetAllPhotographersAsync();
            return Ok(photographers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching photographers");
            return StatusCode(500, new { message = "An error occurred while fetching photographers" });
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PhotographerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PhotographerDto>> GetPhotographer(int id)
    {
        try
        {
            _logger.LogInformation("Fetching photographer {PhotographerId}", id);
            var photographer = await _photographerService.GetPhotographerByIdAsync(id);
            
            if (photographer == null)
                return NotFound(new { message = "Photographer not found" });

            return Ok(photographer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching photographer {PhotographerId}", id);
            return StatusCode(500, new { message = "An error occurred while fetching photographer" });
        }
    }

    [HttpPut("profile")]
    [Authorize(Roles = "Photographer")]
    [ProducesResponseType(typeof(PhotographerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PhotographerDto>> UpdatePhotographerProfile([FromBody] UpdatePhotographerDto updateDto)
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user ID" });
            }

            _logger.LogInformation("User {UserId} updating photographer profile", userId);
            
            var photographer = await _photographerService.UpdatePhotographerAsync(userId, updateDto);
            
            if (photographer == null)
                return NotFound(new { message = "Photographer profile not found" });

            return Ok(photographer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating photographer profile");
            return StatusCode(500, new { message = "An error occurred while updating photographer profile" });
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoBooker.Application.DTOs.Portfolios;
using PhotoBooker.Application.Interfaces;

namespace PhotoBooker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PortfoliosController : ControllerBase
{
    private readonly IPortfolioService _portfolioService;
    private readonly ILogger<PortfoliosController> _logger;

    public PortfoliosController(
        IPortfolioService portfolioService,
        ILogger<PortfoliosController> logger)
    {
        _portfolioService = portfolioService;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<PortfolioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PortfolioDto>>> GetAllPortfolios()
    {
        try
        {
            _logger.LogInformation("Fetching all portfolios");
            var portfolios = await _portfolioService.GetAllPortfoliosAsync();
            return Ok(portfolios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching portfolios");
            return StatusCode(500, new { message = "An error occurred while fetching portfolios" });
        }
    }

    [HttpGet("my-portfolios")]
    [Authorize(Roles = "Photographer")]
    [ProducesResponseType(typeof(IEnumerable<PortfolioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<PortfolioDto>>> GetMyPortfolios()
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user ID" });
            }

            _logger.LogInformation("Fetching portfolios for photographer {PhotographerId}", userId);
            var portfolios = await _portfolioService.GetPhotographerPortfoliosAsync(userId);
            return Ok(portfolios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching photographer portfolios");
            return StatusCode(500, new { message = "An error occurred while fetching portfolios" });
        }
    }

    [HttpGet("photographer/{photographerId}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<PortfolioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PortfolioDto>>> GetPhotographerPortfolios(int photographerId)
    {
        try
        {
            _logger.LogInformation("Fetching portfolios for photographer {PhotographerId}", photographerId);
            var portfolios = await _portfolioService.GetPhotographerPortfoliosAsync(photographerId);
            return Ok(portfolios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching photographer portfolios");
            return StatusCode(500, new { message = "An error occurred while fetching portfolios" });
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PortfolioWithImagesDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PortfolioWithImagesDto>> GetPortfolio(int id)
    {
        try
        {
            _logger.LogInformation("Fetching portfolio {PortfolioId} with images", id);
            var portfolio = await _portfolioService.GetPortfolioWithImagesByIdAsync(id);
            
            if (portfolio == null)
                return NotFound(new { message = "Portfolio not found" });

            return Ok(portfolio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching portfolio {PortfolioId}", id);
            return StatusCode(500, new { message = "An error occurred while fetching portfolio" });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Photographer")]
    [ProducesResponseType(typeof(PortfolioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PortfolioDto>> CreatePortfolio([FromBody] CreatePortfolioDto createDto)
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user ID" });
            }

            _logger.LogInformation("Photographer {PhotographerId} creating portfolio", userId);
            
            var portfolio = await _portfolioService.CreatePortfolioAsync(userId, createDto);
            
            return CreatedAtAction(nameof(GetPortfolio), new { id = portfolio.Id }, portfolio);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Portfolio creation failed: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating portfolio");
            return StatusCode(500, new { message = "An error occurred while creating portfolio" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Photographer")]
    [ProducesResponseType(typeof(PortfolioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PortfolioDto>> UpdatePortfolio(int id, [FromBody] UpdatePortfolioDto updateDto)
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user ID" });
            }

            _logger.LogInformation("Photographer {PhotographerId} updating portfolio {PortfolioId}", userId, id);
            
            var portfolio = await _portfolioService.UpdatePortfolioAsync(id, userId, updateDto);
            
            if (portfolio == null)
                return NotFound(new { message = "Portfolio not found" });

            return Ok(portfolio);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Portfolio update unauthorized: {Message}", ex.Message);
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating portfolio {PortfolioId}", id);
            return StatusCode(500, new { message = "An error occurred while updating portfolio" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Photographer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeletePortfolio(int id)
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user ID" });
            }

            _logger.LogInformation("Photographer {PhotographerId} deleting portfolio {PortfolioId}", userId, id);
            
            var deleted = await _portfolioService.DeletePortfolioAsync(id, userId);
            
            if (!deleted)
                return NotFound(new { message = "Portfolio not found" });

            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Portfolio deletion unauthorized: {Message}", ex.Message);
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting portfolio {PortfolioId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting portfolio" });
        }
    }

    [HttpPost("{id}/images")]
    [Authorize(Roles = "Photographer")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(PortfolioImageDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PortfolioImageDto>> UploadImage(int id, IFormFile file, int displayOrder = 0)
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user ID" });
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded" });
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(new { message = "Only JPG and PNG files are allowed" });
            }

            if (file.Length > 20 * 1024 * 1024)
            {
                return BadRequest(new { message = "File size must be less than 20MB" });
            }

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "portfolios");
            Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = $"http://localhost:5037/uploads/portfolios/{fileName}";

            _logger.LogInformation("Photographer {PhotographerId} uploading image to portfolio {PortfolioId}", userId, id);
            
            var image = await _portfolioService.AddImageToPortfolioAsync(id, userId, imageUrl, displayOrder);
            
            return CreatedAtAction(nameof(GetPortfolio), new { id }, image);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Image upload failed: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Image upload unauthorized: {Message}", ex.Message);
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading image to portfolio {PortfolioId}", id);
            return StatusCode(500, new { message = "An error occurred while uploading image" });
        }
    }

    [HttpDelete("images/{imageId}")]
    [Authorize(Roles = "Photographer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteImage(int imageId)
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user ID" });
            }

            _logger.LogInformation("Photographer {PhotographerId} deleting image {ImageId}", userId, imageId);
            
            var deleted = await _portfolioService.DeleteImageFromPortfolioAsync(imageId, userId);
            
            if (!deleted)
                return NotFound(new { message = "Image not found" });

            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Image deletion unauthorized: {Message}", ex.Message);
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting image {ImageId}", imageId);
            return StatusCode(500, new { message = "An error occurred while deleting image" });
        }
    }
}

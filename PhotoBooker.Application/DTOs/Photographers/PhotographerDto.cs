using PhotoBooker.Domain.Enums;

namespace PhotoBooker.Application.DTOs.Photographers;

public class PhotographerDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public DateTime CreatedAt { get; set; }
}

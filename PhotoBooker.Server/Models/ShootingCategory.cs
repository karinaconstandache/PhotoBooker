namespace PhotoBooker.Server.Models;

public class ShootingCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Navigation properties
    public ICollection<Package> Packages { get; set; } = new List<Package>();
}

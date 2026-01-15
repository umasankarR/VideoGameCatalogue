using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class VideoGame : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public string Developer { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public Genre Genre { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Rating { get; set; } 
    public string CoverImageUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}


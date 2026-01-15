using Domain.Enums;

namespace Application.DTOs;

public class VideoGameDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public string Developer { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public Genre Genre { get; set; }
    public string GenreName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string CoverImageUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}


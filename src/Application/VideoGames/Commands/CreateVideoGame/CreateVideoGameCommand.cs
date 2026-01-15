using Application.DTOs;
using Domain.Enums;
using MediatR;

namespace Application.VideoGames.Commands.CreateVideoGame;

/// <summary>
/// Command to create a new video game
/// </summary>
public class CreateVideoGameCommand : IRequest<VideoGameDto>
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
}


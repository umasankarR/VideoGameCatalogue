using Application.Common;
using Application.DTOs;
using MediatR;

namespace Application.VideoGames.Queries.GetAllVideoGames;

/// <summary>
/// Query to get paginated video games
/// Used for browsing page with pagination support
/// </summary>
public class GetAllVideoGamesQuery : IRequest<PagedResult<VideoGameDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}


using Application.DTOs;
using MediatR;

namespace Application.VideoGames.Queries.GetVideoGameById;

/// <summary>
/// Query to get a single video game by ID
/// </summary>
public class GetVideoGameByIdQuery : IRequest<VideoGameDto?>
{
    public long Id { get; set; }

    public GetVideoGameByIdQuery(long id)
    {
        Id = id;
    }
}


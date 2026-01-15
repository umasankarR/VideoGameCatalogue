using Application.Common;
using Application.DTOs;
using Application.VideoGames.Queries.GetAllVideoGames;
using MediatR;
using WebAPI.Endpoints;

namespace WebAPI.Endpoints.VideoGames;

/// <summary>
/// Endpoint for getting paginated video games (Browse Page)
/// </summary>
public sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/videogames", async (
            int? pageNumber,
            int? pageSize,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAllVideoGamesQuery
            {
                PageNumber = pageNumber ?? 1,
                PageSize = pageSize ?? 10
            };
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(result);
        })
        .WithName("GetAllVideoGames")
        .WithTags("VideoGames")
        .Produces<PagedResult<VideoGameDto>>(StatusCodes.Status200OK)
        .WithSummary("Get paginated video games")
        .WithDescription("Retrieves video games with pagination support. Use pageNumber and pageSize query parameters.");
    }
}


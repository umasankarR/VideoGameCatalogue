using Application.DTOs;
using Application.VideoGames.Queries.GetVideoGameById;
using MediatR;
using WebAPI.Endpoints;

namespace WebAPI.Endpoints.VideoGames;

/// <summary>
/// Endpoint for getting a single video game by ID (Edit Page - Load)
/// </summary>
public sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/videogames/{id:long}", async (
            long id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var query = new GetVideoGameByIdQuery(id);
            var result = await mediator.Send(query, cancellationToken);

            return result is not null
                ? Results.Ok(result)
                : Results.NotFound(new { message = $"Video game with ID {id} not found" });
        })
        .WithName("GetVideoGameById")
        .WithTags("VideoGames")
        .Produces<VideoGameDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Get video game by ID")
        .WithDescription("Retrieves a specific video game for editing");
    }
}


using Application.VideoGames.Commands.DeleteVideoGame;
using MediatR;
using WebAPI.Endpoints;

namespace WebAPI.Endpoints.VideoGames;

/// <summary>
/// Endpoint for deleting a video game
/// </summary>
public sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/videogames/{id:long}", async (
            long id,
            IMediator mediator,
            ILogger<Delete> logger,
            CancellationToken cancellationToken) =>
        {
            logger.LogInformation("Deleting video game with ID: {Id}", id);

            var command = new DeleteVideoGameCommand(id);
            var result = await mediator.Send(command, cancellationToken);

            return result
                ? Results.NoContent()
                : Results.NotFound(new { message = $"Video game with ID {id} not found" });
        })
        .WithName("DeleteVideoGame")
        .WithTags("VideoGames")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Delete a video game")
        .WithDescription("Deletes a video game from the catalogue");
    }
}


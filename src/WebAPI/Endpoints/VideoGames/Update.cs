using Application.DTOs;
using Application.VideoGames.Commands.UpdateVideoGame;
using MediatR;
using WebAPI.Endpoints;

namespace WebAPI.Endpoints.VideoGames;

/// <summary>
/// Endpoint for updating an existing video game (Edit Page - Save)
/// </summary>
public sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/videogames/{id:long}", async (
            long id,
            UpdateVideoGameDto dto,
            IMediator mediator,
            ILogger<Update> logger,
            CancellationToken cancellationToken) =>
        {
            if (id != dto.Id)
            {
                logger.LogWarning("ID mismatch: URL ID {UrlId} vs Body ID {BodyId}", id, dto.Id);
                return Results.BadRequest(new { message = "ID in URL does not match ID in request body" });
            }

            var command = new UpdateVideoGameCommand
            {
                Id = dto.Id,
                Title = dto.Title,
                Publisher = dto.Publisher,
                Developer = dto.Developer,
                ReleaseDate = dto.ReleaseDate,
                Genre = dto.Genre,
                Price = dto.Price,
                Description = dto.Description,
                Rating = dto.Rating,
                CoverImageUrl = dto.CoverImageUrl,
                IsActive = dto.IsActive
            };

            var result = await mediator.Send(command, cancellationToken);

            return result is not null
                ? Results.Ok(result)
                : Results.NotFound(new { message = $"Video game with ID {id} not found" });
        })
        .WithName("UpdateVideoGame")
        .WithTags("VideoGames")
        .Produces<VideoGameDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Update a video game")
        .WithDescription("Updates an existing video game entry");
    }
}


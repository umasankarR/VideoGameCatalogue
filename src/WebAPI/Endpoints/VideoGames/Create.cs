using Application.DTOs;
using Application.VideoGames.Commands.CreateVideoGame;
using MediatR;
using WebAPI.Endpoints;

namespace WebAPI.Endpoints.VideoGames;

/// <summary>
/// Endpoint for creating a new video game
/// </summary>
public sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/videogames", async (
            CreateVideoGameDto dto,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateVideoGameCommand
            {
                Title = dto.Title,
                Publisher = dto.Publisher,
                Developer = dto.Developer,
                ReleaseDate = dto.ReleaseDate,
                Genre = dto.Genre,
                Price = dto.Price,
                Description = dto.Description,
                Rating = dto.Rating,
                CoverImageUrl = dto.CoverImageUrl
            };

            var result = await mediator.Send(command, cancellationToken);

            return Results.Created($"/api/videogames/{result.Id}", result);
        })
        .WithName("CreateVideoGame")
        .WithTags("VideoGames")
        .Produces<VideoGameDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Create a new video game")
        .WithDescription("Creates a new video game entry in the catalogue");
    }
}


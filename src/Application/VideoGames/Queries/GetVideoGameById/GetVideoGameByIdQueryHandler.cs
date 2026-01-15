using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.VideoGames.Queries.GetVideoGameById;

public class GetVideoGameByIdQueryHandler : IRequestHandler<GetVideoGameByIdQuery, VideoGameDto?>
{
    private readonly IVideoGameRepository _repository;

    public GetVideoGameByIdQueryHandler(IVideoGameRepository repository)
    {
        _repository = repository;
    }

    public async Task<VideoGameDto?> Handle(GetVideoGameByIdQuery request, CancellationToken cancellationToken)
    {
        var videoGame = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (videoGame == null)
            return null;

        return new VideoGameDto
        {
            Id = videoGame.Id,
            Title = videoGame.Title,
            Publisher = videoGame.Publisher,
            Developer = videoGame.Developer,
            ReleaseDate = videoGame.ReleaseDate,
            Genre = videoGame.Genre,
            GenreName = videoGame.Genre.ToString(),
            Price = videoGame.Price,
            Description = videoGame.Description,
            Rating = videoGame.Rating,
            CoverImageUrl = videoGame.CoverImageUrl,
            IsActive = videoGame.IsActive,
            CreatedAt = videoGame.CreatedAt,
            UpdatedAt = videoGame.UpdatedAt
        };
    }
}


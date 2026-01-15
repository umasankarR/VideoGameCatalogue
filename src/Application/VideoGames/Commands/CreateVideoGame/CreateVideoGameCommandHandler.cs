using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.VideoGames.Commands.CreateVideoGame;

public class CreateVideoGameCommandHandler : IRequestHandler<CreateVideoGameCommand, VideoGameDto>
{
    private readonly IVideoGameRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVideoGameCommandHandler(IVideoGameRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<VideoGameDto> Handle(CreateVideoGameCommand request, CancellationToken cancellationToken)
    {
        var videoGame = new VideoGame
        {
            Title = request.Title,
            Publisher = request.Publisher,
            Developer = request.Developer,
            ReleaseDate = request.ReleaseDate,
            Genre = request.Genre,
            Price = request.Price,
            Description = request.Description,
            Rating = request.Rating,
            CoverImageUrl = request.CoverImageUrl,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(videoGame, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new VideoGameDto
        {
            Id = created.Id,
            Title = created.Title,
            Publisher = created.Publisher,
            Developer = created.Developer,
            ReleaseDate = created.ReleaseDate,
            Genre = created.Genre,
            GenreName = created.Genre.ToString(),
            Price = created.Price,
            Description = created.Description,
            Rating = created.Rating,
            CoverImageUrl = created.CoverImageUrl,
            IsActive = created.IsActive,
            CreatedAt = created.CreatedAt,
            UpdatedAt = created.UpdatedAt
        };
    }
}


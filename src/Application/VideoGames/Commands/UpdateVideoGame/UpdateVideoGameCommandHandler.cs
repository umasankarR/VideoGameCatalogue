using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.VideoGames.Commands.UpdateVideoGame;

public class UpdateVideoGameCommandHandler : IRequestHandler<UpdateVideoGameCommand, VideoGameDto?>
{
    private readonly IVideoGameRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVideoGameCommandHandler(IVideoGameRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<VideoGameDto?> Handle(UpdateVideoGameCommand request, CancellationToken cancellationToken)
    {
        var videoGame = await _repository.GetByIdForUpdateAsync(request.Id, cancellationToken);
        if (videoGame == null)
            return null;

        videoGame.Title = request.Title;
        videoGame.Publisher = request.Publisher;
        videoGame.Developer = request.Developer;
        videoGame.ReleaseDate = request.ReleaseDate;
        videoGame.Genre = request.Genre;
        videoGame.Price = request.Price;
        videoGame.Description = request.Description;
        videoGame.Rating = request.Rating;
        videoGame.CoverImageUrl = request.CoverImageUrl;
        videoGame.IsActive = request.IsActive;
        videoGame.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(videoGame, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new VideoGameDto
        {
            Id = updated.Id,
            Title = updated.Title,
            Publisher = updated.Publisher,
            Developer = updated.Developer,
            ReleaseDate = updated.ReleaseDate,
            Genre = updated.Genre,
            GenreName = updated.Genre.ToString(),
            Price = updated.Price,
            Description = updated.Description,
            Rating = updated.Rating,
            CoverImageUrl = updated.CoverImageUrl,
            IsActive = updated.IsActive,
            CreatedAt = updated.CreatedAt,
            UpdatedAt = updated.UpdatedAt
        };
    }
}


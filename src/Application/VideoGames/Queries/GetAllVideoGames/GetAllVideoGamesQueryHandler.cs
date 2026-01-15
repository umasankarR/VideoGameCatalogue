using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.VideoGames.Queries.GetAllVideoGames;

public class GetAllVideoGamesQueryHandler : IRequestHandler<GetAllVideoGamesQuery, PagedResult<VideoGameDto>>
{
    private readonly IVideoGameRepository _repository;

    public GetAllVideoGamesQueryHandler(IVideoGameRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<VideoGameDto>> Handle(GetAllVideoGamesQuery request, CancellationToken cancellationToken)
    {
        var (videoGames, totalCount) = await _repository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var items = videoGames.Select(vg => new VideoGameDto
        {
            Id = vg.Id,
            Title = vg.Title,
            Publisher = vg.Publisher,
            Developer = vg.Developer,
            ReleaseDate = vg.ReleaseDate,
            Genre = vg.Genre,
            GenreName = vg.Genre.ToString(),
            Price = vg.Price,
            Description = vg.Description,
            Rating = vg.Rating,
            CoverImageUrl = vg.CoverImageUrl,
            IsActive = vg.IsActive,
            CreatedAt = vg.CreatedAt,
            UpdatedAt = vg.UpdatedAt
        });

        return new PagedResult<VideoGameDto>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}


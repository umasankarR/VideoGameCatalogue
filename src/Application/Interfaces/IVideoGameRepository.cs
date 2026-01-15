using Domain.Entities;

namespace Application.Interfaces;

public interface IVideoGameRepository
{
    Task<IEnumerable<VideoGame>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<(IEnumerable<VideoGame> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<VideoGame?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<VideoGame> AddAsync(VideoGame videoGame, CancellationToken cancellationToken = default);
    Task<VideoGame> UpdateAsync(VideoGame videoGame, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
}


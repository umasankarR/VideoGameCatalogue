using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class VideoGameRepository : IVideoGameRepository
{
    private readonly ApplicationDbContext _context;

    public VideoGameRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<VideoGame>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.VideoGames
            .AsNoTracking()
            .Where(vg => vg.IsActive)
            .OrderByDescending(vg => vg.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<VideoGame> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var baseQuery = _context.VideoGames
            .AsNoTracking()
            .Where(vg => vg.IsActive);

        var items = await baseQuery
            .OrderByDescending(vg => vg.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<VideoGame?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.VideoGames
            .AsNoTracking()
            .FirstOrDefaultAsync(vg => vg.Id == id, cancellationToken);
    }

    public async Task<VideoGame?> GetByIdForUpdateAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.VideoGames
            .FirstOrDefaultAsync(vg => vg.Id == id, cancellationToken);
    }

    public async Task<VideoGame> AddAsync(VideoGame videoGame, CancellationToken cancellationToken = default)
    {
        await _context.VideoGames.AddAsync(videoGame, cancellationToken);
        return videoGame;
    }

    public async Task<VideoGame> UpdateAsync(VideoGame videoGame, CancellationToken cancellationToken = default)
    {
        _context.VideoGames.Update(videoGame);
        return await Task.FromResult(videoGame);
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var videoGame = await GetByIdForUpdateAsync(id, cancellationToken);

        if (videoGame == null)
            return false;

        _context.VideoGames.Remove(videoGame);
        return true;
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.VideoGames
            .AnyAsync(vg => vg.Id == id, cancellationToken);
    }
}


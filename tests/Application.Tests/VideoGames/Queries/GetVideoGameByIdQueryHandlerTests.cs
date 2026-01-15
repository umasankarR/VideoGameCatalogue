using Application.Interfaces;
using Application.VideoGames.Queries.GetVideoGameById;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Tests.VideoGames.Queries;

public class GetVideoGameByIdQueryHandlerTests
{
    private readonly Mock<IVideoGameRepository> _repositoryMock;
    private readonly GetVideoGameByIdQueryHandler _handler;

    public GetVideoGameByIdQueryHandlerTests()
    {
        _repositoryMock = new Mock<IVideoGameRepository>();
        _handler = new GetVideoGameByIdQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnVideoGameDto_WhenGameExists()
    {
        var gameId = 1L;
        var game = CreateVideoGame(gameId, "Test Game", Genre.Action);

        _repositoryMock
            .Setup(r => r.GetByIdAsync(gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        var query = new GetVideoGameByIdQuery(gameId);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(gameId);
        result.Title.Should().Be("Test Game");
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenGameDoesNotExist()
    {
        var gameId = 999L;

        _repositoryMock
            .Setup(r => r.GetByIdAsync(gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((VideoGame?)null);

        var query = new GetVideoGameByIdQuery(gameId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldMapAllProperties_Correctly()
    {
        var gameId = 1L;
        var releaseDate = new DateTime(2024, 6, 15);
        var createdAt = DateTime.UtcNow.AddDays(-10);
        var updatedAt = DateTime.UtcNow.AddDays(-5);

        var game = new VideoGame
        {
            Id = gameId,
            Title = "The Legend of Test",
            Publisher = "Test Publisher",
            Developer = "Test Studio",
            ReleaseDate = releaseDate,
            Genre = Genre.RPG,
            Price = 69.99m,
            Description = "An epic adventure",
            Rating = 95,
            CoverImageUrl = "http://example.com/cover.png",
            IsActive = true,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        var query = new GetVideoGameByIdQuery(gameId);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(gameId);
        result.Title.Should().Be("The Legend of Test");
        result.Publisher.Should().Be("Test Publisher");
        result.Developer.Should().Be("Test Studio");
        result.ReleaseDate.Should().Be(releaseDate);
        result.Genre.Should().Be(Genre.RPG);
        result.GenreName.Should().Be("RPG");
        result.Price.Should().Be(69.99m);
        result.Description.Should().Be("An epic adventure");
        result.Rating.Should().Be(95);
        result.CoverImageUrl.Should().Be("http://example.com/cover.png");
        result.IsActive.Should().BeTrue();
        result.CreatedAt.Should().Be(createdAt);
        result.UpdatedAt.Should().Be(updatedAt);
    }

    [Fact]
    public async Task Handle_ShouldCallRepository_WithCorrectId()
    {
        var gameId = 1L;

        _repositoryMock
            .Setup(r => r.GetByIdAsync(gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((VideoGame?)null);

        var query = new GetVideoGameByIdQuery(gameId);

        await _handler.Handle(query, CancellationToken.None);

        _repositoryMock.Verify(r => r.GetByIdAsync(gameId, It.IsAny<CancellationToken>()), Times.Once);
    }

    private static VideoGame CreateVideoGame(long id, string title, Genre genre)
    {
        return new VideoGame
        {
            Id = id,
            Title = title,
            Publisher = "Publisher",
            Developer = "Developer",
            ReleaseDate = DateTime.UtcNow,
            Genre = genre,
            Price = 49.99m,
            Description = "Description",
            Rating = 80,
            CoverImageUrl = "http://example.com/cover.jpg",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }
}

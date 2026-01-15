using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Application.VideoGames.Queries.GetAllVideoGames;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Tests.VideoGames.Queries;

public class GetAllVideoGamesQueryHandlerTests
{
    private readonly Mock<IVideoGameRepository> _repositoryMock;
    private readonly GetAllVideoGamesQueryHandler _handler;

    public GetAllVideoGamesQueryHandlerTests()
    {
        _repositoryMock = new Mock<IVideoGameRepository>();
        _handler = new GetAllVideoGamesQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPagedResult_WhenGamesExist()
    {
        // Arrange
        var games = new List<VideoGame>
        {
            CreateVideoGame("Game 1", Genre.Action),
            CreateVideoGame("Game 2", Genre.RPG)
        };

        _repositoryMock
            .Setup(r => r.GetPagedAsync(1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync((games, 2));

        var query = new GetAllVideoGamesQuery { PageNumber = 1, PageSize = 10 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyResult_WhenNoGamesExist()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetPagedAsync(1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync((new List<VideoGame>(), 0));

        var query = new GetAllVideoGamesQuery { PageNumber = 1, PageSize = 10 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ShouldMapAllProperties_Correctly()
    {
        // Arrange
        var game = CreateVideoGame("Test Game", Genre.Adventure);
        game.Publisher = "Test Publisher";
        game.Developer = "Test Developer";
        game.Price = 59.99m;
        game.Rating = 85;

        _repositoryMock
            .Setup(r => r.GetPagedAsync(1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync((new List<VideoGame> { game }, 1));

        var query = new GetAllVideoGamesQuery { PageNumber = 1, PageSize = 10 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var dto = result.Items.First();
        dto.Title.Should().Be("Test Game");
        dto.Publisher.Should().Be("Test Publisher");
        dto.Developer.Should().Be("Test Developer");
        dto.Genre.Should().Be(Genre.Adventure);
        dto.GenreName.Should().Be("Adventure");
        dto.Price.Should().Be(59.99m);
        dto.Rating.Should().Be(85);
    }

    [Fact]
    public async Task Handle_ShouldCalculateTotalPages_Correctly()
    {
        // Arrange
        var games = new List<VideoGame> { CreateVideoGame("Game 1", Genre.Action) };

        _repositoryMock
            .Setup(r => r.GetPagedAsync(1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync((games, 25));

        var query = new GetAllVideoGamesQuery { PageNumber = 1, PageSize = 10 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.TotalPages.Should().Be(3); // 25 items / 10 per page = 3 pages
    }

    private static VideoGame CreateVideoGame(string title, Genre genre)
    {
        return new VideoGame
        {
            Id = 1,
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

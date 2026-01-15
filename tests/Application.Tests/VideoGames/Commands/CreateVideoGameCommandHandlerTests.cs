using Application.Interfaces;
using Application.VideoGames.Commands.CreateVideoGame;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Tests.VideoGames.Commands;

public class CreateVideoGameCommandHandlerTests
{
    private readonly Mock<IVideoGameRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateVideoGameCommandHandler _handler;

    public CreateVideoGameCommandHandlerTests()
    {
        _repositoryMock = new Mock<IVideoGameRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateVideoGameCommandHandler(_repositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateVideoGame_AndReturnDto()
    {
        // Arrange
        var command = new CreateVideoGameCommand
        {
            Title = "New Game",
            Publisher = "Test Publisher",
            Developer = "Test Developer",
            ReleaseDate = new DateTime(2024, 12, 1),
            Genre = Genre.Action,
            Price = 59.99m,
            Description = "A new exciting game",
            Rating = 85,
            CoverImageUrl = "http://example.com/cover.jpg"
        };

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<VideoGame>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((VideoGame game, CancellationToken _) =>
            {
                game.Id = 1; // Simulate database auto-increment
                return game;
            });

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("New Game");
        result.Publisher.Should().Be("Test Publisher");
        result.Developer.Should().Be("Test Developer");
        result.Genre.Should().Be(Genre.Action);
        result.GenreName.Should().Be("Action");
        result.Price.Should().Be(59.99m);
        result.IsActive.Should().BeTrue();
        result.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Handle_ShouldGenerateId_ForId()
    {
        // Arrange
        var command = CreateCommand();
        VideoGame? capturedGame = null;

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<VideoGame>(), It.IsAny<CancellationToken>()))
            .Callback<VideoGame, CancellationToken>((game, _) => capturedGame = game)
            .ReturnsAsync((VideoGame game, CancellationToken _) => game);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedGame.Should().NotBeNull();
        capturedGame!.Id.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task Handle_ShouldSetCreatedAt_ToUtcNow()
    {
        // Arrange
        var command = CreateCommand();
        var beforeTest = DateTime.UtcNow;
        VideoGame? capturedGame = null;

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<VideoGame>(), It.IsAny<CancellationToken>()))
            .Callback<VideoGame, CancellationToken>((game, _) => capturedGame = game)
            .ReturnsAsync((VideoGame game, CancellationToken _) => game);

        // Act
        await _handler.Handle(command, CancellationToken.None);
        var afterTest = DateTime.UtcNow;

        // Assert
        capturedGame.Should().NotBeNull();
        capturedGame!.CreatedAt.Should().BeOnOrAfter(beforeTest);
        capturedGame.CreatedAt.Should().BeOnOrBefore(afterTest);
    }

    [Fact]
    public async Task Handle_ShouldSetIsActive_ToTrue()
    {
        // Arrange
        var command = CreateCommand();
        VideoGame? capturedGame = null;

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<VideoGame>(), It.IsAny<CancellationToken>()))
            .Callback<VideoGame, CancellationToken>((game, _) => capturedGame = game)
            .ReturnsAsync((VideoGame game, CancellationToken _) => game);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedGame.Should().NotBeNull();
        capturedGame!.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldCallRepository_AddAsync()
    {
        // Arrange
        var command = CreateCommand();

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<VideoGame>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((VideoGame game, CancellationToken _) => game);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<VideoGame>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCallUnitOfWork_SaveChangesAsync()
    {
        // Arrange
        var command = CreateCommand();

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<VideoGame>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((VideoGame game, CancellationToken _) => game);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static CreateVideoGameCommand CreateCommand()
    {
        return new CreateVideoGameCommand
        {
            Title = "Test Game",
            Publisher = "Publisher",
            Developer = "Developer",
            ReleaseDate = DateTime.UtcNow,
            Genre = Genre.Action,
            Price = 49.99m,
            Description = "Description",
            Rating = 80,
            CoverImageUrl = "http://example.com/cover.jpg"
        };
    }
}

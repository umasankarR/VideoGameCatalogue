using Application.Interfaces;
using Application.VideoGames.Commands.UpdateVideoGame;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Tests.VideoGames.Commands;

public class UpdateVideoGameCommandHandlerTests
{
    private readonly Mock<IVideoGameRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateVideoGameCommandHandler _handler;

    public UpdateVideoGameCommandHandlerTests()
    {
        _repositoryMock = new Mock<IVideoGameRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new UpdateVideoGameCommandHandler(_repositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateVideoGame_WhenGameExists()
    {
        // Arrange
        var gameId = 1L;
        var existingGame = CreateExistingGame(gameId);

        var command = new UpdateVideoGameCommand
        {
            Id = gameId,
            Title = "Updated Title",
            Publisher = "Updated Publisher",
            Developer = "Updated Developer",
            ReleaseDate = new DateTime(2025, 1, 1),
            Genre = Genre.RPG,
            Price = 79.99m,
            Description = "Updated Description",
            Rating = 95,
            CoverImageUrl = "http://example.com/updated.jpg",
            IsActive = true
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingGame);

        _repositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<VideoGame>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((VideoGame game, CancellationToken _) => game);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Updated Title");
        result.Publisher.Should().Be("Updated Publisher");
        result.Developer.Should().Be("Updated Developer");
        result.Genre.Should().Be(Genre.RPG);
        result.Price.Should().Be(79.99m);
        result.Rating.Should().Be(95);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenGameDoesNotExist()
    {
        // Arrange
        var gameId = 999L;
        var command = CreateCommand(gameId);

        _repositoryMock
            .Setup(r => r.GetByIdAsync(gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((VideoGame?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldSetUpdatedAt_ToUtcNow()
    {
        // Arrange
        var gameId = 1L;
        var existingGame = CreateExistingGame(gameId);
        var command = CreateCommand(gameId);
        var beforeTest = DateTime.UtcNow;

        _repositoryMock
            .Setup(r => r.GetByIdAsync(gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingGame);

        _repositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<VideoGame>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((VideoGame game, CancellationToken _) => game);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        var afterTest = DateTime.UtcNow;

        // Assert
        result.Should().NotBeNull();
        result!.UpdatedAt.Should().NotBeNull();
        result.UpdatedAt!.Value.Should().BeOnOrAfter(beforeTest);
        result.UpdatedAt.Value.Should().BeOnOrBefore(afterTest);
    }

    [Fact]
    public async Task Handle_ShouldNotCallSaveChanges_WhenGameDoesNotExist()
    {
        // Arrange
        var gameId = 999L;
        var command = CreateCommand(gameId);

        _repositoryMock
            .Setup(r => r.GetByIdAsync(gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((VideoGame?)null);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldCallSaveChanges_WhenGameExists()
    {
        // Arrange
        var gameId = 1L;
        var existingGame = CreateExistingGame(gameId);
        var command = CreateCommand(gameId);

        _repositoryMock
            .Setup(r => r.GetByIdAsync(gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingGame);

        _repositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<VideoGame>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((VideoGame game, CancellationToken _) => game);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldUpdateIsActive_WhenChanged()
    {
        // Arrange
        var gameId = 1L;
        var existingGame = CreateExistingGame(gameId);
        existingGame.IsActive = true;

        var command = CreateCommand(gameId);
        command.IsActive = false;

        _repositoryMock
            .Setup(r => r.GetByIdAsync(gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingGame);

        _repositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<VideoGame>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((VideoGame game, CancellationToken _) => game);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.IsActive.Should().BeFalse();
    }

    private static VideoGame CreateExistingGame(long id)
    {
        return new VideoGame
        {
            Id = id,
            Title = "Original Title",
            Publisher = "Original Publisher",
            Developer = "Original Developer",
            ReleaseDate = DateTime.UtcNow.AddYears(-1),
            Genre = Genre.Action,
            Price = 49.99m,
            Description = "Original Description",
            Rating = 80,
            CoverImageUrl = "http://example.com/original.jpg",
            IsActive = true,
            CreatedAt = DateTime.UtcNow.AddMonths(-6)
        };
    }

    private static UpdateVideoGameCommand CreateCommand(long id)
    {
        return new UpdateVideoGameCommand
        {
            Id = id,
            Title = "Updated Title",
            Publisher = "Updated Publisher",
            Developer = "Updated Developer",
            ReleaseDate = DateTime.UtcNow,
            Genre = Genre.RPG,
            Price = 59.99m,
            Description = "Updated Description",
            Rating = 85,
            CoverImageUrl = "http://example.com/updated.jpg",
            IsActive = true
        };
    }
}

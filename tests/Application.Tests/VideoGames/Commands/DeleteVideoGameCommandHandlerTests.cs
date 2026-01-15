using Application.Interfaces;
using Application.VideoGames.Commands.DeleteVideoGame;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Tests.VideoGames.Commands;

public class DeleteVideoGameCommandHandlerTests
{
    private readonly Mock<IVideoGameRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteVideoGameCommandHandler _handler;

    public DeleteVideoGameCommandHandlerTests()
    {
        _repositoryMock = new Mock<IVideoGameRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new DeleteVideoGameCommandHandler(_repositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenGameDeletedSuccessfully()
    {
        // Arrange
        var gameId = 1L;
        var command = new DeleteVideoGameCommand(gameId);

        _repositoryMock
            .Setup(r => r.DeleteAsync(gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenGameDoesNotExist()
    {
        // Arrange
        var gameId = 999L;
        var command = new DeleteVideoGameCommand(gameId);

        _repositoryMock
            .Setup(r => r.DeleteAsync(gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldCallSaveChanges_WhenDeleteSucceeds()
    {
        // Arrange
        var gameId = 1L;
        var command = new DeleteVideoGameCommand(gameId);

        _repositoryMock
            .Setup(r => r.DeleteAsync(gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotCallSaveChanges_WhenDeleteFails()
    {
        // Arrange
        var gameId = 999L;
        var command = new DeleteVideoGameCommand(gameId);

        _repositoryMock
            .Setup(r => r.DeleteAsync(gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldCallRepository_WithCorrectId()
    {
        // Arrange
        var gameId = 1L;
        var command = new DeleteVideoGameCommand(gameId);

        _repositoryMock
            .Setup(r => r.DeleteAsync(gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.DeleteAsync(gameId, It.IsAny<CancellationToken>()), Times.Once);
    }
}

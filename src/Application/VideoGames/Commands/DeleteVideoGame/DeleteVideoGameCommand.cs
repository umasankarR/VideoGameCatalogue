using MediatR;

namespace Application.VideoGames.Commands.DeleteVideoGame;

/// <summary>
/// Command to delete a video game
/// </summary>
public class DeleteVideoGameCommand : IRequest<bool>
{
    public long Id { get; set; }

    public DeleteVideoGameCommand(long id)
    {
        Id = id;
    }
}


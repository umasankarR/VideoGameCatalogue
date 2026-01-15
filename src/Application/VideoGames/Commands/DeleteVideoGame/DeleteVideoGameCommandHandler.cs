using Application.Interfaces;
using MediatR;

namespace Application.VideoGames.Commands.DeleteVideoGame;

public class DeleteVideoGameCommandHandler : IRequestHandler<DeleteVideoGameCommand, bool>
{
    private readonly IVideoGameRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVideoGameCommandHandler(IVideoGameRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteVideoGameCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.DeleteAsync(request.Id, cancellationToken);
        
        if (result)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return result;
    }
}


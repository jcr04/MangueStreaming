using System.Threading;
using System.Threading.Tasks;

namespace MangueStreaming.Application.Interfaces
{
    public interface ICommandHandler<TCommand, TResult>
    {
        Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
    }
}

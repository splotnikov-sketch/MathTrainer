using System.Threading.Tasks;

namespace MathTrainer.Core.Base
{
    public interface ICommandHandler<in TCommand, out TResult>
        where TCommand : ICommand<TResult>
    {
        TResult Execute(TCommand command);
    }

    public interface ICommandHandler<in TCommand>
        where TCommand : ICommand
    {
        Task Execute(TCommand command);
    }
}
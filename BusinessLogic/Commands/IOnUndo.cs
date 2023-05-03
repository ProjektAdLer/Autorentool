namespace BusinessLogic.Commands;

public interface IOnUndo
{
    public event Action<ICommand> OnUndo;
}
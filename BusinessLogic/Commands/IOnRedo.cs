namespace BusinessLogic.Commands;

public interface IOnRedo
{
    public event Action<ICommand> OnRedo;
}
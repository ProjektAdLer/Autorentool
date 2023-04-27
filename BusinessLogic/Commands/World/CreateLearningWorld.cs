using BusinessLogic.Entities;
using Shared.Extensions;

namespace BusinessLogic.Commands.World;

public class CreateLearningWorld : IUndoCommand
{
    public string Name => nameof(CreateLearningWorld);
    internal AuthoringToolWorkspace AuthoringToolWorkspace { get; }
    private readonly Action<AuthoringToolWorkspace> _mappingAction;

    private IMemento? _memento;
    internal LearningWorld LearningWorld { get; }

    public CreateLearningWorld(AuthoringToolWorkspace authoringToolWorkspace, string name, string shortname,
        string authors,
        string language, string description, string goals, Action<AuthoringToolWorkspace> mappingAction)
    {
        LearningWorld = new LearningWorld(name, shortname, authors, language, description, goals);
        AuthoringToolWorkspace = authoringToolWorkspace;
        _mappingAction = mappingAction;
    }
    
    public CreateLearningWorld(AuthoringToolWorkspace authoringToolWorkspace, LearningWorld learningWorld,
        Action<AuthoringToolWorkspace> mappingAction)
    {
        LearningWorld = learningWorld;
        AuthoringToolWorkspace = authoringToolWorkspace;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = AuthoringToolWorkspace.GetMemento();

        if (AuthoringToolWorkspace.LearningWorlds.Any(lw => lw.Name == LearningWorld.Name))
        {
            LearningWorld.Name = StringHelper.GetUniqueName(AuthoringToolWorkspace.LearningWorlds.Select(lw => lw.Name),
                LearningWorld.Name);
        }
        AuthoringToolWorkspace.LearningWorlds.Add(LearningWorld);

        _mappingAction.Invoke(AuthoringToolWorkspace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        AuthoringToolWorkspace.RestoreMemento(_memento);

        _mappingAction.Invoke(AuthoringToolWorkspace);
    }

    public void Redo() => Execute();
}
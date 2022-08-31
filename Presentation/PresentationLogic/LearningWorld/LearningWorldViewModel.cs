﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;

namespace Presentation.PresentationLogic.LearningWorld;

public class LearningWorldViewModel : ILearningWorldViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LearningWorldViewModel"/> class.
    /// </summary>
    /// <param name="name">The name of the learning world.</param>
    /// <param name="shortname">The short name (abbreviation) of the learning world.</param>
    /// <param name="authors">The string containing the names of all the authors working on the learning world.</param>
    /// <param name="language">The primary language used in this learning world.</param>
    /// <param name="description">A description of the learning world and its contents.</param>
    /// <param name="goals">A description of the goals this learning world is supposed to achieve.</param>
    /// <param name="unsavedChanges">Whether or not the object contains changes that are yet to be saved to disk.</param>
    /// <param name="learningElements">Optional collection of learning elements contained in the learning world.
    /// Should be used when loading a saved learning world into the application.</param>
    /// <param name="learningSpaces">Optional collection of learning spaces contained in the learning world.
    /// Should be used when loading a saved learnign world into the application.</param>
    public LearningWorldViewModel(string name, string shortname, string authors, string language, string description,
        string goals, bool unsavedChanges = true, ICollection<ILearningElementViewModel>? learningElements = null,
        ICollection<ILearningSpaceViewModel>? learningSpaces = null)
    {
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Language = language;
        Description = description;
        Goals = goals;
        UnsavedChanges = unsavedChanges;
        LearningElements = learningElements ?? new Collection<ILearningElementViewModel>();
        LearningSpaces = learningSpaces ?? new Collection<ILearningSpaceViewModel>();
    }
    public const string fileEnding = "awf";
    
    private ICollection<ILearningElementViewModel> _learningElements;
    private ICollection<ILearningSpaceViewModel> _learningSpaces;
    private string _name;
    private string _shortname;
    private string _authors;
    private string _language;
    private string _description;
    private string _goals;
    private bool _unsavedChanges;
    private ILearningObjectViewModel? _selectedLearningObject;
    private bool _showingLearningSpaceView;

    public string FileEnding => fileEnding;

    public ICollection<ILearningElementViewModel> LearningElements
    {
        get => _learningElements;
        set
        {
            if (!SetField(ref _learningElements, value)) return;
            OnPropertyChanged(nameof(LearningObjects));
            OnPropertyChanged(nameof(Workload));
        }
    }

    public ICollection<ILearningSpaceViewModel> LearningSpaces
    {
        get => _learningSpaces;
        set
        {
            if (!SetField(ref _learningSpaces, value)) return;
            OnPropertyChanged(nameof(LearningObjects));
            OnPropertyChanged(nameof(Workload));
        }
    }

    public IEnumerable<ILearningObjectViewModel> LearningObjects => LearningElements.Concat<ILearningObjectViewModel>(LearningSpaces);
    public int Workload =>
        LearningSpaces.Sum(space => space.Workload) + LearningElements.Sum(element => element.Workload);

    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    public string Shortname
    {
        get => _shortname;
        set => SetField(ref _shortname, value);
    }

    public string Authors
    {
        get => _authors;
        set => SetField(ref _authors, value);
    }

    public string Language
    {
        get => _language;
        set => SetField(ref _language, value);
    }

    public string Description
    {
        get => _description;
        set => SetField(ref _description, value);
    }

    public string Goals
    {
        get => _goals;
        set => SetField(ref _goals, value);
    }

    public bool UnsavedChanges
    {
        get => _unsavedChanges;
        set => SetField(ref _unsavedChanges, value);
    }

    public ILearningObjectViewModel? SelectedLearningObject
    {
        get => _selectedLearningObject;
        set => SetField(ref _selectedLearningObject, value);
    }

    public bool ShowingLearningSpaceView
    {
        get => _showingLearningSpaceView;
        set => SetField(ref _showingLearningSpaceView, value);
    }
    

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
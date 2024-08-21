using System.Windows;
using HammerTime.Views;
using HammerTime.Models;
using HammerTime.Commands;
using System.Windows.Input;
using HammerTime.Services.Data;
using HammerTime.Commands.Base;
using HammerTime.ViewModels.Base;
using System.Collections.ObjectModel;
using HammerTime.Services.Notification;

namespace HammerTime.ViewModels;

internal class SessionViewModel : ViewModelBase
{
    private ObservableCollection<Project> projectData;
    public ObservableCollection<Project> ProjectData
    {
        get { return projectData; }
        private set 
        {
            projectData = value;
            OnPropertyChanged(nameof(ProjectData));
        }
    }

    private Project? selectedProject;
    public Project SelectedProject
    {
        get { return selectedProject!; }
        set 
        { 
            selectedProject = value;
            OnPropertyChanged(nameof(SelectedProject));
            OnPropertyChanged(nameof(IsProjectSelected));
        }
    }

    public bool IsProjectSelected => selectedProject != null;

    public IWindowFactory Window { get; private set; }
    private readonly ProjectDataService projectDataService;
    private readonly NotificationService notificationService = new();

    public ICommand DeselectDataGridRowCommand { get; } = new DeselectDataGridRowCommand();
    public ICommand DeleteSelectedProjectsCommand => new RelayCommand(DeleteSelectedProject);
    public ICommand DeleteAllProjectsCommand => new RelayCommand(DeleteAllProjects);


    public SessionViewModel(IWindowFactory window, ProjectDataService projectDataService)
    {
        this.projectDataService = projectDataService;
        this.projectDataService.OnProjectAdded += OnProjectAdded;

        Window = window;

        projectData = new(projectDataService.LoadProjects());
        projectData.CollectionChanged += OnCollectionChanged;
    }

    private void OnCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        projectDataService.SaveProjects([.. projectData]);
    }

    private void OnProjectAdded(Project project)
    {
        if (TryGetExistingProject(project, out Project existingProject))
        {
            existingProject.Update(project);
            projectDataService.SaveProjects([.. projectData]);
        }
        else
        {
            projectData.Add(project);
        }

        ShowSessionSavedNotification();
    }

    private void DeleteSelectedProject(object? parameter)
    {
        if (!ConfirmDeletion()) return;

        projectData.Remove(selectedProject!);
        ShowSessionDeletedNotification();
    }

    private void DeleteAllProjects(object? parameter)
    {
        if (!ConfirmDeletion()) return;

        ProjectData.Clear();
        ShowSessionDeletedNotification();
    }

    private bool ConfirmDeletion()
    {
        MessageBoxResult result = MessageBox.Show("This will not delete you project(s)\n" +
            "Are you sure you want to delete the selected session(s)?",
                                          "Confirm Deletion",
                                          MessageBoxButton.YesNo, MessageBoxImage.Warning);

        return result == MessageBoxResult.Yes;
    }

    private bool TryGetExistingProject(Project project, out Project existingProject)
    {
        existingProject = projectData.FirstOrDefault(x => x.Name == project.Name)!;
        return existingProject != null;
    }

    public void ShowSessionSavedNotification()
    {
        notificationService.Push("Session Saved", "Your session has been successfully saved.");
    }

    public void ShowSessionDeletedNotification()
    {
        notificationService.Push("Session(s) Deleted", "The selected session(s) have been successfully deleted.");
    }

    public override void Dispose()
    {
        projectDataService.OnProjectAdded -= OnProjectAdded;
        projectData.CollectionChanged -= OnCollectionChanged;
    }
}

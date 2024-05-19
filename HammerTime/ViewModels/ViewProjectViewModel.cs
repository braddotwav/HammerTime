using HammerTime.Views;
using HammerTime.Models;
using HammerTime.Commands;
using HammerTime.Services;
using System.Windows.Input;
using HammerTime.ViewModels.Base;
using System.Collections.ObjectModel;

namespace HammerTime.ViewModels
{
    internal class ViewProjectViewModel : ViewModelBase
    {
        private ObservableCollection<ProjectData> projectData;

        public ObservableCollection<ProjectData> ProjectData
        {
            get { return projectData; }
            private set 
            {
                projectData = value;
                OnPropertyChanged(nameof(projectData));
            }
        }

        public IWindowFactory Window { get; private set; }
        private readonly IData data;

        #region Commands
        public ICommand DeselectDataGridRowCommand { get; }
        #endregion

        public ViewProjectViewModel(IWindowFactory window, IData data)
        {
            this.data = data;
            DeselectDataGridRowCommand = new DeselectDataGridRowCommand();

            Window = window;
            Window.OnWindowOpened += OnWindowOpened;
            Window.OnWindowClosed += OnWindowClosed;

            data.OnSaveData += OnSaveData;

            projectData = new(data.GetData());
        }

        private void OnSaveData()
        {
            ProjectData = new(data.GetData());
        }

        private void OnWindowOpened()
        {
            projectData.CollectionChanged += OnCollectionChanged;
        }

        private void OnWindowClosed()
        {
            projectData.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            data.Update(projectData.ToList());
        }

        public override void Dispose()
        {
            Window.OnWindowOpened -= OnWindowOpened;
            Window.OnWindowClosed -= OnWindowClosed;
            data.OnSaveData -= OnSaveData;
        }
    }
}

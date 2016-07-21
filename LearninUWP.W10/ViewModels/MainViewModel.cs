using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Windows.Input;
using AppStudio.Uwp;
using AppStudio.Uwp.Actions;
using AppStudio.Uwp.Navigation;
using AppStudio.Uwp.Commands;
using AppStudio.DataProviders;

using AppStudio.DataProviders.Rss;
using AppStudio.DataProviders.Menu;
using AppStudio.DataProviders.Html;
using AppStudio.DataProviders.LocalStorage;
using LearninUWP.Sections;


namespace LearninUWP.ViewModels
{
    public class MainViewModel : PageViewModelBase
    {
        public ListViewModel Sessions { get; private set; }
        public ListViewModel Speakers { get; private set; }
        public ListViewModel Gallery { get; private set; }
        public ListViewModel Channel9Rss { get; private set; }
        public ListViewModel Information { get; private set; }

        public MainViewModel(int visibleItems) : base()
        {
            Title = "LearninUWP";
            Sessions = ViewModelFactory.NewList(new SessionsSection(), visibleItems);
            Speakers = ViewModelFactory.NewList(new SpeakersSection(), visibleItems);
            Gallery = ViewModelFactory.NewList(new GallerySection(), visibleItems);
            Channel9Rss = ViewModelFactory.NewList(new Channel9RssSection(), visibleItems);
            Information = ViewModelFactory.NewList(new InformationSection());

            if (GetViewModels().Any(vm => !vm.HasLocalData))
            {
                Actions.Add(new ActionInfo
                {
                    Command = RefreshCommand,
                    Style = ActionKnownStyles.Refresh,
                    Name = "RefreshButton",
                    ActionType = ActionType.Primary
                });
            }
        }

		#region Commands
		public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    var refreshDataTasks = GetViewModels()
                        .Where(vm => !vm.HasLocalData).Select(vm => vm.LoadDataAsync(true));

                    await Task.WhenAll(refreshDataTasks);
					LastUpdated = GetViewModels().OrderBy(vm => vm.LastUpdated, OrderType.Descending).FirstOrDefault()?.LastUpdated;
                    OnPropertyChanged("LastUpdated");
                });
            }
        }
		#endregion

        public async Task LoadDataAsync()
        {
            var loadDataTasks = GetViewModels().Select(vm => vm.LoadDataAsync());

            await Task.WhenAll(loadDataTasks);
			LastUpdated = GetViewModels().OrderBy(vm => vm.LastUpdated, OrderType.Descending).FirstOrDefault()?.LastUpdated;
            OnPropertyChanged("LastUpdated");
        }

        private IEnumerable<ListViewModel> GetViewModels()
        {
            yield return Sessions;
            yield return Speakers;
            yield return Gallery;
            yield return Channel9Rss;
            yield return Information;
        }
    }
}

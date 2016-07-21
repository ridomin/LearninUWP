using System;
using System.Collections.Generic;
using AppStudio.Uwp;
using AppStudio.Uwp.Commands;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Input;
using LearninUWP.Services;
using LearninUWP.Sections;
namespace LearninUWP.ViewModels
{
    public class FavoritesViewModel : PageViewModelBase
    {
        public ListViewModel Sessions { get; private set; }
        public ListViewModel Speakers { get; private set; }

        public FavoritesViewModel()
        {
			Title = "My Event";

            Sessions = ViewModelFactory.NewList(new SessionsSection());
            Speakers = ViewModelFactory.NewList(new SpeakersSection());

			ShowRoamingWarning = Singleton<UserFavorites>.Instance.RoamingQuota == 0;                       
        }     

		#region	ShowRoamingWarning
        private bool _showRoamingWarning;

        public bool ShowRoamingWarning
        {
            get { return _showRoamingWarning; }
            set { SetProperty(ref _showRoamingWarning, value); }
        }
		#endregion

		#region	HasItems
		private bool _hasItems = true;

        public bool HasItems
        {
            get { return _hasItems; }
            set { SetProperty(ref _hasItems, value); }
        }
		#endregion

        public async Task LoadDataAsync()
        {
            this.HasItems = true;
            List<Task> loadDataTasks = new List<Task>();

            if (Singleton<UserFavorites>.Instance.Sections != null)
            {
                foreach (var favInSection in Singleton<UserFavorites>.Instance.Sections)
                {
                    var vm = GetSectionViewModel(favInSection.Name);

                    if (vm != null)
                    {
                        loadDataTasks.Add(vm.FilterDataAsync(favInSection.ItemsId));
                    }
                } 
            }

            await Task.WhenAll(loadDataTasks);
            this.HasItems = GetViewModels().Any(vm => vm.HasItems);
        }

        private IEnumerable<ListViewModel> GetViewModels()
        {
            yield return Sessions;
            yield return Speakers;
        }

		private ListViewModel GetSectionViewModel(string sectionName)
        {
            return GetViewModels().FirstOrDefault(vm => vm.SectionName == sectionName);
        }
    }
}

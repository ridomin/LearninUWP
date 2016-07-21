using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.LocalStorage;
using AppStudio.Uwp;
using Windows.ApplicationModel.Appointments;
using System.Linq;

using LearninUWP.Navigation;
using LearninUWP.ViewModels;

namespace LearninUWP.Sections
{
    public class SpeakersSection : Section<Speakers1Schema>
    {
		private LocalStorageDataProvider<Speakers1Schema> _dataProvider;

		public SpeakersSection()
		{
			_dataProvider = new LocalStorageDataProvider<Speakers1Schema>();
		}

		public override async Task<IEnumerable<Speakers1Schema>> GetDataAsync(SchemaBase connectedItem = null)
        {
            var config = new LocalStorageDataConfig
            {
                FilePath = "/Assets/Data/Speakers.json",
            };
            return await _dataProvider.LoadDataAsync(config, MaxRecords);
        }

        public override async Task<IEnumerable<Speakers1Schema>> GetNextPageAsync()
        {
            return await _dataProvider.LoadMoreDataAsync();
        }

        public override bool HasMorePages
        {
            get
            {
                return _dataProvider.HasMoreItems;
            }
        }

        public override bool NeedsNetwork
        {
            get
            {
                return false;
            }
        }

        public override ListPageConfig<Speakers1Schema> ListPage
        {
            get 
            {
                return new ListPageConfig<Speakers1Schema>
                {
                    Title = "Speakers",

                    Page = typeof(Pages.SpeakersListPage),

                    LayoutBindings = (viewModel, item) =>
                    {
                        viewModel.Title = item.Name.ToSafeString();
                        viewModel.SubTitle = item.Description.ToSafeString();
                        viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.Image.ToSafeString());
                    },
                    DetailNavigation = (item) =>
                    {
						return NavInfo.FromPage<Pages.SpeakersDetailPage>(true);
                    }
                };
            }
        }

        public override DetailPageConfig<Speakers1Schema> DetailPage
        {
            get
            {
                var bindings = new List<Action<ItemViewModel, Speakers1Schema>>();
                bindings.Add((viewModel, item) =>
                {
                    viewModel.PageTitle = "Speaker";
                    viewModel.Title = item.Name.ToSafeString();
                    viewModel.Description = item.Description.ToSafeString();
                    viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.Image.ToSafeString());
                    viewModel.Content = null;
                });

                var actions = new List<ActionConfig<Speakers1Schema>>
                {
                    ActionConfig<Speakers1Schema>.Link("Web Profile", (item) => item.Url.ToSafeString()),
                };

                return new DetailPageConfig<Speakers1Schema>
                {
                    Title = "Speakers",
                    LayoutBindings = bindings,
                    Actions = actions
                };
            }
        }
    }
}

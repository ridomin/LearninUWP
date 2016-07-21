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
    public class GallerySection : Section<Gallery1Schema>
    {
		private LocalStorageDataProvider<Gallery1Schema> _dataProvider;

		public GallerySection()
		{
			_dataProvider = new LocalStorageDataProvider<Gallery1Schema>();
		}

		public override async Task<IEnumerable<Gallery1Schema>> GetDataAsync(SchemaBase connectedItem = null)
        {
            var config = new LocalStorageDataConfig
            {
                FilePath = "/Assets/Data/Gallery.json",
            };
            return await _dataProvider.LoadDataAsync(config, MaxRecords);
        }

        public override async Task<IEnumerable<Gallery1Schema>> GetNextPageAsync()
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

        public override ListPageConfig<Gallery1Schema> ListPage
        {
            get 
            {
                return new ListPageConfig<Gallery1Schema>
                {
                    Title = "Gallery",

                    Page = typeof(Pages.GalleryListPage),

                    LayoutBindings = (viewModel, item) =>
                    {
                        viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.Image.ToSafeString());
                    },
                    DetailNavigation = (item) =>
                    {
						return NavInfo.FromPage<Pages.GalleryDetailPage>(true);
                    }
                };
            }
        }

        public override DetailPageConfig<Gallery1Schema> DetailPage
        {
            get
            {
                var bindings = new List<Action<ItemViewModel, Gallery1Schema>>();
                bindings.Add((viewModel, item) =>
                {
                    viewModel.PageTitle = "Event gallery";
                    viewModel.Title = item.Title.ToSafeString();
                    viewModel.Description = item.Description.ToSafeString();
                    viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.Image.ToSafeString());
                    viewModel.Content = null;
                });

                var actions = new List<ActionConfig<Gallery1Schema>>
                {
                };

                return new DetailPageConfig<Gallery1Schema>
                {
                    Title = "Gallery",
                    LayoutBindings = bindings,
                    Actions = actions
                };
            }
        }
    }
}

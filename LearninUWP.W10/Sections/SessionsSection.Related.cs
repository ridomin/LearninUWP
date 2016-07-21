using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.LocalStorage;

using LearninUWP.Navigation;
using LearninUWP.ViewModels;

namespace LearninUWP.Sections
{
    class SessionsSectionRelated : Section<Speakers1Schema>
    {
		private LocalStorageDataProvider<Speakers1Schema> _dataProvider;

		public SessionsSectionRelated()
        {
            _dataProvider = new LocalStorageDataProvider<Speakers1Schema>();
        }

        public override async Task<IEnumerable<Speakers1Schema>> GetDataAsync(SchemaBase connectedItem = null)
        {
            var selected = connectedItem as Sessions1Schema;
			if(selected == null)
			{
				return new Speakers1Schema[0];
			}

			var config = new LocalStorageDataConfig
            {
                FilePath = "/Assets/Data/Speakers.json"
            };
			//avoid pagination because in memory filter
			var result = await _dataProvider.LoadDataAsync(config, int.MaxValue);
			return result
					.Where(r => r.Name.ToSafeString() == selected.Speaker.ToSafeString())
					.ToList();
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

        public override ListPageConfig<Speakers1Schema> ListPage
        {
            get 
            {
                return new ListPageConfig<Speakers1Schema>
                {
                    Title = "Speaker",

                    LayoutBindings = (viewModel, item) =>
					{
						viewModel.Title = item.Name.ToSafeString();
						viewModel.SubTitle = item.Description.ToSafeString();
						viewModel.Description = null;
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
                return null;
            }
        }
    }
}

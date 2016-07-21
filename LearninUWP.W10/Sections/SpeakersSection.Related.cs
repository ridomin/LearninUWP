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
    class SpeakersSectionRelated : Section<Sessions1Schema>
    {
		private LocalStorageDataProvider<Sessions1Schema> _dataProvider;

		public SpeakersSectionRelated()
        {
            _dataProvider = new LocalStorageDataProvider<Sessions1Schema>();
        }

        public override async Task<IEnumerable<Sessions1Schema>> GetDataAsync(SchemaBase connectedItem = null)
        {
            var selected = connectedItem as Speakers1Schema;
			if(selected == null)
			{
				return new Sessions1Schema[0];
			}

			var config = new LocalStorageDataConfig
            {
                FilePath = "/Assets/Data/Sessions.json"
            };
			//avoid pagination because in memory filter
			var result = await _dataProvider.LoadDataAsync(config, int.MaxValue);
			return result
					.Where(r => r.Speaker.ToSafeString() == selected.Name.ToSafeString())
					.ToList();
        }

        public override async Task<IEnumerable<Sessions1Schema>> GetNextPageAsync()
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

        public override ListPageConfig<Sessions1Schema> ListPage
        {
            get 
            {
                return new ListPageConfig<Sessions1Schema>
                {
                    Title = "Sessions",

                    LayoutBindings = (viewModel, item) =>
					{
						viewModel.Title = item.Title.ToSafeString();
						viewModel.SubTitle = item.Description.ToSafeString();
						viewModel.Description = null;
						viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.Image.ToSafeString());
					},
                    DetailNavigation = (item) =>
                    {
						return NavInfo.FromPage<Pages.SessionsDetailPage>(true);
                    }
                };
            }
        }

        public override DetailPageConfig<Sessions1Schema> DetailPage
        {
            get
            {
                return null;
            }
        }
    }
}

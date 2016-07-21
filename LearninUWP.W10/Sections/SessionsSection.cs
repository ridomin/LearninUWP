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
    public class SessionsSection : Section<Sessions1Schema>
    {
		private LocalStorageDataProvider<Sessions1Schema> _dataProvider;

		public SessionsSection()
		{
			_dataProvider = new LocalStorageDataProvider<Sessions1Schema>();
		}

		public override async Task<IEnumerable<Sessions1Schema>> GetDataAsync(SchemaBase connectedItem = null)
        {
            var config = new LocalStorageDataConfig
            {
                FilePath = "/Assets/Data/Sessions.json",
            };
            return await _dataProvider.LoadDataAsync(config, MaxRecords);
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

        public override bool NeedsNetwork
        {
            get
            {
                return false;
            }
        }

        public override ListPageConfig<Sessions1Schema> ListPage
        {
            get 
            {
                return new ListPageConfig<Sessions1Schema>
                {
                    Title = "Sessions",

                    Page = typeof(Pages.SessionsListPage),

                    LayoutBindings = (viewModel, item) =>
                    {
						viewModel.Header = item.SessionDate.ToString(DateTimeFormat.DayOfWeek);
                        viewModel.Title = item.Title.ToSafeString();
                        viewModel.SubTitle = item.Description.ToSafeString();
                        viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.Image.ToSafeString());
						viewModel.Aside = item.SessionDate.ToString(DateTimeFormat.CardTime);
						viewModel.Footer = item.Speaker.ToSafeString();

						viewModel.GroupBy = item.SessionDate.SafeType().Date;

						viewModel.OrderBy = item.SessionDate;
                    },
					OrderType = OrderType.Ascending,
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
                var bindings = new List<Action<ItemViewModel, Sessions1Schema>>();
                bindings.Add((viewModel, item) =>
                {
                    viewModel.PageTitle = "Sessions";
                    viewModel.Title = item.Title.ToSafeString();
                    viewModel.Description = item.Description.ToSafeString();
                    viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.Image.ToSafeString());
                    viewModel.Content = null;
                });

                var actions = new List<ActionConfig<Sessions1Schema>>
                {
                    ActionConfig<Sessions1Schema>.Link("Open web", (item) => item.Url.ToSafeString()),
                    ActionConfig<Sessions1Schema>.AddToCalendar("Add to calendar", (item) => new Windows.ApplicationModel.Appointments.Appointment() {StartTime = item.SessionDate.SafeType() == DateTime.MinValue ? DateTime.Now : item.SessionDate.SafeType(), Subject = item.Title.ToSafeString(), AllDay = false }),
                };

                return new DetailPageConfig<Sessions1Schema>
                {
                    Title = "Sessions",
                    LayoutBindings = bindings,
                    Actions = actions
                };
            }
        }
    }
}

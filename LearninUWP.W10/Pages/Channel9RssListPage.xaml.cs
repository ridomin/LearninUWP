//---------------------------------------------------------------------------
//
// <copyright file="Channel9RssListPage.xaml.cs" company="Microsoft">
//    Copyright (C) 2015 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <createdOn>7/21/2016 6:04:32 PM</createdOn>
//
//---------------------------------------------------------------------------

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using AppStudio.DataProviders.Rss;
using LearninUWP.Sections;
using LearninUWP.ViewModels;
using AppStudio.Uwp;

namespace LearninUWP.Pages
{
    public sealed partial class Channel9RssListPage : Page
    {
	    public ListViewModel ViewModel { get; set; }
        public Channel9RssListPage()
        {
			ViewModel = ViewModelFactory.NewList(new Channel9RssSection());

            this.InitializeComponent();
			commandBar.DataContext = ViewModel;
			NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
			ShellPage.Current.ShellControl.SelectItem("cb162b3e-1a87-4ce5-9e38-3fe879213d84");
			ShellPage.Current.ShellControl.SetCommandBar(commandBar);
			if (e.NavigationMode == NavigationMode.New)
            {			
				await this.ViewModel.LoadDataAsync();
                this.ScrollToTop();
			}			
            base.OnNavigatedTo(e);
        }

    }
}

//---------------------------------------------------------------------------
//
// <copyright file="SpeakersListPage.xaml.cs" company="Microsoft">
//    Copyright (C) 2015 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <createdOn>7/21/2016 6:04:32 PM</createdOn>
//
//---------------------------------------------------------------------------

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using AppStudio.DataProviders.LocalStorage;
using LearninUWP.Sections;
using LearninUWP.ViewModels;
using AppStudio.Uwp;

namespace LearninUWP.Pages
{
    public sealed partial class SpeakersListPage : Page
    {
	    public ListViewModel ViewModel { get; set; }
        public SpeakersListPage()
        {
			ViewModel = ViewModelFactory.NewList(new SpeakersSection());

            this.InitializeComponent();
			commandBar.DataContext = ViewModel;
			NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
			ShellPage.Current.ShellControl.SelectItem("64274a5d-de9a-4cf0-812f-8cae50fb9edc");
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

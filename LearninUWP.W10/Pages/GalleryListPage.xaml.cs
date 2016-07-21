//---------------------------------------------------------------------------
//
// <copyright file="GalleryListPage.xaml.cs" company="Microsoft">
//    Copyright (C) 2015 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <createdOn>7/21/2016 7:59:56 PM</createdOn>
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
    public sealed partial class GalleryListPage : Page
    {
	    public ListViewModel ViewModel { get; set; }
        public GalleryListPage()
        {
			ViewModel = ViewModelFactory.NewList(new GallerySection());

            this.InitializeComponent();
			commandBar.DataContext = ViewModel;
			NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
			ShellPage.Current.ShellControl.SelectItem("fc2541d8-69bb-4591-8e6b-e95c22be38b3");
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

//---------------------------------------------------------------------------
//
// <copyright file="GalleryDetailPage.xaml.cs" company="Microsoft">
//    Copyright (C) 2015 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <createdOn>7/21/2016 6:04:32 PM</createdOn>
//
//---------------------------------------------------------------------------

using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using AppStudio.DataProviders.LocalStorage;
using LearninUWP.Sections;
using LearninUWP.Navigation;
using LearninUWP.ViewModels;
using AppStudio.Uwp;

namespace LearninUWP.Pages
{
    public sealed partial class GalleryDetailPage : Page
    {
        private DataTransferManager _dataTransferManager;

        public GalleryDetailPage()
        {
            ViewModel = ViewModelFactory.NewDetail(new GallerySection());
            this.InitializeComponent();
			commandBar.DataContext = ViewModel;
        }

        public DetailViewModel ViewModel { get; set; }        

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadStateAsync(e.Parameter as NavDetailParameter);

            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += OnDataRequested;
            ShellPage.Current.SupportFullScreen = true;

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _dataTransferManager.DataRequested -= OnDataRequested;
            ShellPage.Current.SupportFullScreen = false;

            base.OnNavigatedFrom(e);
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            ViewModel.ShareContent(args.Request);
        }
    }
}

//using Microsoft.AspNetCore.SignalR.Client;

using ChatAppMobile.ViewModels;

namespace ChatAppMobile
{
    public partial class MainPage : ContentPage
    {
        MainViewModel vm;
        MainViewModel VM
        {
            get => vm ?? (vm = (MainViewModel)BindingContext);
        }
        public MainPage()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            VM.ConnectCommand.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            VM.DisconnectCommand.Execute(null);
        }
    }
}
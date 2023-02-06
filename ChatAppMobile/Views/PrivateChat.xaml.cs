using ChatAppMobile.ViewModels;

namespace ChatAppMobile.Views;

public partial class PrivateChat : ContentPage
{
    PrivateChatViewModel vm;
    PrivateChatViewModel VM
    {
        get => vm ?? (vm = (PrivateChatViewModel)BindingContext);
    }
    public PrivateChat()
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
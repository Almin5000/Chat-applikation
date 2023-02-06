using ChatAppMobile.ViewModels;

namespace ChatAppMobile.Views;

public partial class GroupChatView : ContentPage
{
    ChatViewModel vm;
    ChatViewModel VM
    {
        get => vm ?? (vm = (ChatViewModel)BindingContext);
    }
    public GroupChatView()
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
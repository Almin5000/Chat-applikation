using ChatAppMobile.Model;
using ChatAppMobile.ViewModels;
using ChatAppMobile.Views;

namespace ChatAppMobile
{
    public partial class MainPage : ContentPage
    {
        LobbyViewModel vm;
        LobbyViewModel VM
        {
            get => vm ?? (vm = (LobbyViewModel)BindingContext);
        }
        public MainPage()
        {
            InitializeComponent();

        }

        //async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{

        //    await VM.GoToGroupChat(Navigation, e.SelectedItem as string);
        //    ((ListView)sender).SelectedItem = null;
        //}

        async private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as RoomsModel;
            await VM.GoToGroupChat(Navigation, item.Room as string);
            ((ListView)sender).SelectedItem = null;
        }

        private INavigation GetNavigation()
        {
            return Navigation;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PrivateChat());
        }
    }
}
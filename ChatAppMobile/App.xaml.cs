using ChatAppMobile.Views;

namespace ChatAppMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
            //MainPage = new AppShell();
            //MainPage = new GroupChatView();

        }
    }
}
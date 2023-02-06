using ChatAppMobile.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatAppMobile.Helpers;
using Settings = ChatAppMobile.Helpers.Settings;
using System.Collections.ObjectModel;
using ChatAppMobile.Model;
using Microsoft.Maui.ApplicationModel.Communication;

namespace ChatAppMobile.ViewModels
{
    public class LobbyViewModel : BaseViewModel
    {
        //public ObservableCollection<RoomsModel> Rooms2 { get; }
        public List<string> Rooms { get; }
        public List<RoomsModel> Rooms2 { get; }
        public string Room { get; set; }
        public Command PrivatChat { get; }
        public LobbyViewModel()
        {
            ////HER FRA
            Rooms2 = new List<RoomsModel>();

            Rooms2.Add(new RoomsModel("Group Room 1"));
            Rooms2.Add(new RoomsModel("Group Room 2"));
            Rooms2.Add(new RoomsModel("Group Room 3"));
            Rooms2.Add(new RoomsModel("Group Room 4"));

            //PrivatChat = new Command(async () => await GoToPrivateChat());

            // //HER TIL

            Rooms = new List<string>
            {
                "Room 1",
                "Room 2",
                "Room 3",
                "Room 4"
            };

            //ObservableCollection<RoomsModel> mList = new ObservableCollection<RoomsModel>();

            //foreach (var item in Rooms)
            //    mList.Add(item);

            //getRooms();

        }

        public LobbyViewModel(INavigation navigation)
        {


        }
        public string UserName
        {
            get => Settings.UserName;
            set
            {
                if (value == UserName)
                    return;
                Settings.UserName = value;
                OnPropertyChanged();
            }
        }



        public async Task GoToGroupChat(INavigation navigation, string group)
        {
            if (string.IsNullOrWhiteSpace(group))
                return;

            if (string.IsNullOrWhiteSpace(UserName))
                return;

            Settings.Group = group;
            await navigation.PushAsync(new GroupChatView());
        }

        //public async Task GoToPrivateChat()
        //{

        //    if (string.IsNullOrWhiteSpace(UserName))
        //        return;

        //    await Navigation.PushAsync(new PrivateChat());

        //}

    }
}

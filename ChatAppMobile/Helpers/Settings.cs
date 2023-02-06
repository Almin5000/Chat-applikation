using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppMobile.Helpers
{
    public static class Settings
    {
        public static string UserName
        {
            get => Preferences.Get(nameof(UserName), string.Empty);
            set => Preferences.Set(nameof(UserName), value);
        }

        public static string Group
        {
            get => Preferences.Get(nameof(Group), string.Empty);
            set => Preferences.Set(nameof(Group), value);
        }
        public static string ConnectionID
        {
            get => Preferences.Get(nameof(ConnectionID), string.Empty);
            set => Preferences.Set(nameof(ConnectionID), value);
        }
        public static string ReceiverID
        {
            get => Preferences.Get(nameof(ReceiverID), string.Empty);
            set => Preferences.Set(nameof(ReceiverID), value);
        }
    }
}

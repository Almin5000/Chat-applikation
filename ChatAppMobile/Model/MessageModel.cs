using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppMobile.Model
{
    public class MessageModel : ObservableObject
    {
        string user;
        public string User {
            get => user;
            set => SetProperty(ref user, value);
        }
        string message;
        public string Message {
            get => message; 
            set => SetProperty(ref message, value);
        }
        public bool IsOwnerMessage { get; set; }
    }
}

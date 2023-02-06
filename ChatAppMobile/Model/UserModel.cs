using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppMobile.Model
{
    public class UserModel : ObservableObject
    {
        string connectionID;
        public string ConnectionID
        {
            get => connectionID;
            set => SetProperty(ref connectionID, value);
        }
    }
}

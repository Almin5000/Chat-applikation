using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppMobile.Services.Interfaces
{
    public interface IChatService
    {
        Task Connect();
        Task Disconnect();
        Task SendMessage(string user, string message);
        void ReceiveMessage(Action<string, string> GetMessageAndUser);
    }
}

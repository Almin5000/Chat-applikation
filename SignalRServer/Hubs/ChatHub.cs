using Microsoft.AspNetCore.SignalR;

namespace SignalRServer.Hubs
{
    public class MessageDetail
    {
        public int FromUserID { get; set; }
        public string FromUserName { get; set; }
        public int ToUserID { get; set; }
        public string ToUserName { get; set; }
        public string Message { get; set; }
    }

    public class UserDetail
    {
        public string ConnectionId { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
    public class ChatHub : Hub
    {
        static List<UserDetail> ConnectedUsers = new List<UserDetail>();
        static List<MessageDetail> CurrentMessage = new List<MessageDetail>();


        public async Task AddToGroup(string groupName, string user)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("EnteredOrLeft",
                $"{user} has" +
                $" joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName, string user)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("EnteredOrLeft",
                $"{user} has" +
                $" left the group {groupName}.");
        }

        public async Task SendMessageGroup(string groupName, string user, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
        }

        public async Task TypingGroup(string groupName, string user)
        {
            await Clients.Group(groupName).SendAsync("TypingMessage", user);
        }


        public async Task SendMessage(string user, string message)
        {
            Console.WriteLine($"{user}: {message}");
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task Typing(string user)
        {
            await Clients.All.SendAsync("TypingMessage", user);
        }

        public async Task SendPrivateMessage(string sender, string receiver, string message)
        {
            await Clients.Group(receiver).SendAsync("ReceivePrivateMessage", sender, message);
        }

        public async Task SendToUser(string user, string receiverConnectionId, string message)
        {
            await Clients.Client(receiverConnectionId).SendAsync("ReceivePrivateMessage", user, message);
        }

        public async Task GetConnectionId()
        {
            var connectionID = Context.ConnectionId;
            await Clients.Caller.SendAsync("ConnectionID", connectionID);
        }

        //public string GetConnectionId() => Context.ConnectionId;
    }
}

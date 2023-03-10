using ChatAppMobile.Model;
using ChatAppMobile.Services.Core;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;
using ChatAppMobile.Helpers;

namespace ChatAppMobile.ViewModels
{
    public partial class ChatViewModel : BaseViewModel
    {
        HubConnection hubConnection;
        ChatService _chatService = new ChatService();
        public MessageModel MessageModel { get;}
        public ObservableCollection<MessageModel> Messages { get;}

        bool isConnected;

        //public bool IsConnected
        //{
        //    get => isConnected;
        //    set
        //    {
        //        Device.BeginInvokeOnMainThread(() =>
        //        {
        //            SetProperty(ref isConnected, value);
        //        });
        //    }
        //}

        public bool IsConnected
        {
            get => isConnected;
            set
            {
                SetProperty(ref isConnected, value);
            }
        }

        public string Username { get; set; }
        public string Message { get; set; }
        public Command SendMessageCommand { get; }
        public Command ConnectCommand { get; }
        public Command DisconnectCommand { get; }
        Random random;
        public ChatViewModel()
        {
            Title = Settings.Group;

            MessageModel = new MessageModel();
            Messages = new ObservableCollection<MessageModel>();
            SendMessageCommand = new Command(async () => await SendMessage());
            ConnectCommand = new Command(async() => await Connect());
            DisconnectCommand = new Command(async() => await Disconnect());
            random = new Random();
            

            var baseUrl = "http://localhost";

            if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            {
                baseUrl = "http://10.0.2.2";
            }

            hubConnection = new HubConnectionBuilder()
                .WithUrl($"{baseUrl}:5238/chat")
                .Build();

            hubConnection.Closed += async (error) =>
            {
                //SendLocalMessage("Connection Closed");
                isConnected = false;
                await Task.Delay(random.Next(0, 5) * 1000);
                await Connect();
            };

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var finalMessage = $"{user}: {message}";
                SendLocalMessage(finalMessage);
            });

            hubConnection.On<string>("EnteredOrLeft", (message) =>
            {
                SendLocalMessage(message);
            });

        }

        async Task Connect()
        {
            if (IsConnected)
                return;
            try
            {
                await hubConnection.StartAsync();
                await hubConnection.SendAsync("AddToGroup", Settings.Group, Settings.UserName);
                isConnected = true;
                //SendLocalMessage("Connected");
            }
            catch (Exception ex)
            {
                SendLocalMessage($"Connection Error: {ex.Message}");
            }
        }

        async Task Disconnect()
        {
            if (!IsConnected)
                return;

            await hubConnection.SendAsync("RemoveFromGroup", Settings.Group, Settings.UserName);
            await hubConnection.StopAsync();
            isConnected = false;
            //SendLocalMessage("Disconnected");
        }

        async Task SendMessage()
        {
            try
            {
                IsBusy = true;
                await hubConnection.InvokeAsync("SendMessageGroup",
                    Settings.Group,
                    Settings.UserName,
                    MessageModel.Message);
            }
            catch (Exception ex)
            {
                SendLocalMessage($"Send failed: {ex.Message}");
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        //async Task SendMessage()
        //{
        //    try
        //    {
        //        IsBusy = true;
        //        await hubConnection.InvokeAsync("SendMessage", 
        //            MessageModel.User,
        //            MessageModel.Message );
        //    }
        //    catch (Exception ex)
        //    {
        //        SendLocalMessage($"Send failed: {ex.Message}");
        //        throw;
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}

        //private void SendLocalMessage(string message)
        //{
        //    Dispatcher.Dispatch(() =>
        //        Messages.Add(new MessageModel
        //        {
        //            Message = message
        //        }));
        //}

        private void SendLocalMessage(string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Messages.Add(new MessageModel
                {
                    Message = message
                });
            });
        }
    }
}

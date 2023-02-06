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
using System.Net;

namespace ChatAppMobile.ViewModels
{
    public partial class PrivateChatViewModel : BaseViewModel
    {
        HubConnection hubConnection;
        ChatService _chatService = new ChatService();
        public MessageModel MessageModel { get; }
        public ObservableCollection<MessageModel> Messages { get; }
        public UserModel UserModel { get; }
        public ObservableCollection<UserModel> Users { get; }

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

        string connectionID;
        public string ConnectionID
        {
            get => connectionID;
            set
            {
                SetProperty(ref connectionID, value);
            }
        }

        public string Username { get; set; }
        public string Message { get; set; }
        //public string ConnectionID { get; set; }


        public Command SendMessageCommand { get; }
        public Command ConnectCommand { get; }
        public Command DisconnectCommand { get; }
        Random random;
        public PrivateChatViewModel()
        {
            MessageModel = new MessageModel();
            Messages = new ObservableCollection<MessageModel>();
            SendMessageCommand = new Command(async () => await SendToUser());
            ConnectCommand = new Command(async () => await Connect());
            DisconnectCommand = new Command(async () => await Disconnect());
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

            hubConnection.On<string, string>("ReceivePrivateMessage", (user, message) =>
            {
                var finalMessage = $"{user}: {message}";
                SendLocalMessage(finalMessage);
            });

            hubConnection.On<string>("ConnectionID", (message) =>
            {
                GetConnectionID(message);
                //connectionID = message;
                Settings.ConnectionID = message.ToString();
                //SendLocalMessage(message);
            });

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

        public string ReceiverID
        {
            get => Settings.ReceiverID;
            set
            {
                if (value == ReceiverID)
                    return;
                Settings.ReceiverID = value;
                OnPropertyChanged();
            }
        }

        //private void WebClient_DownloadProgressChanged(object sender)
        //{
        //    try
        //    {
        //        connID.Progress = (double)(e.ProgressPercentage / 100.0);
        //        Application.Current.MainPage.Dispatcher.Dispatch(() => progressBarText.Text = String.Format("Downloading {0}%", e.ProgressPercentage));
        //    }
        //    catch (Exception error)
        //    {
        //        throw error;
        //    }
        //}

        async Task Connect()
        {
            if (IsConnected)
                return;
            try
            {
                await hubConnection.StartAsync();
                await hubConnection.SendAsync("GetConnectionId");
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

            await hubConnection.StopAsync();
            isConnected = false;
            //SendLocalMessage("Disconnected");
        }

        async Task SendToUser()
        {
            try
            {
                IsBusy = true;
                await hubConnection.InvokeAsync("SendToUser",
                    Settings.UserName,
                    Settings.ReceiverID,
                    MessageModel.Message);
                SendLocalMessage($"{Settings.UserName}: {MessageModel.Message}");
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

        private void GetConnectionID(string connectionID)
        {
            Application.Current.MainPage.Dispatcher.Dispatch(() =>
            {
                ConnectionID = connectionID;
            });
        }

        private void SendLocalMessage(string message)
        {
            Application.Current.MainPage.Dispatcher.Dispatch(() =>
            {
                Messages.Add(new MessageModel
                {
                    Message = message
                });
            });
        }
    }
}

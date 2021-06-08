using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using System.IO.Ports;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        bool toggle = false;
        private HubConnectionBuilder _hubConnectionBuilder { get; set; }
        private HubConnection hubConnection;
        private HubConnection portConnection;
        private Func<ArduinoData, Task> handleReceiveData;
        private Func<bool, Task> handleSwitchStatus;
        ArduinoData data = new ArduinoData();

        public MainPage()
        {
            InitializeComponent();

            handleReceiveData += ReceiveData;
            portConnection = new HubConnectionBuilder()
                        .WithUrl("https://192.168.3.7:5001/datahub")
                        .WithAutomaticReconnect()
                        .Build();
            portConnection.On("ReceiveData", this.handleReceiveData);

            handleSwitchStatus += ReceiveSwitchStatus;
            hubConnection = new HubConnectionBuilder()
                        .WithUrl("https://192.168.3.7:5001/ctrlhub")
                        .WithAutomaticReconnect()
                        .Build();
            
            hubConnection.On("ReceiveSwitchStatus", this.handleSwitchStatus);
        }

        protected async Task ConnectToPort()
        {
            try
            {
                await portConnection.StartAsync();
            }
            catch (Exception ex)
            {
                label.Text = ex.Message;
            }
        }
        protected async Task ConnectToHub()
        {
            try
            {
                await hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                label.Text = ex.Message;
            }
        }


        private async void connectButton_Clicked(object sender, System.EventArgs e)
        {
            
            await ConnectToHub();
            await ConnectToPort();

            label.Text = $"{portConnection.State} , {hubConnection.State}";

        }

        private async Task SendMessage(bool toggle)
        {
            try
            {
                await hubConnection.InvokeAsync("ReceiveSwitchStatus", toggle);
            }
            catch (Exception ex)
            {
                label.Text = ex.Message;
            }
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            //isStart 반전
            toggle = !toggle;

            if (toggle) //아두이노 실행중
            {
                ((Button)sender).Text = "STOP";
            }
            else //아두이노 sleeping...
            {
                ((Button)sender).Text = "START";
            }

            await SendMessage(toggle);


        }

        Task ReceiveData(ArduinoData recivedData)
        {
            data = recivedData;
            label.Text = $"{data.Date}";
            return Task.CompletedTask;
        }

        Task ReceiveSwitchStatus(bool arg)
        {
            toggle = arg;
            //StateHasChanged();
            return Task.CompletedTask;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.SignalR.Client;

namespace VelesClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HubConnection _connection;
        public MainWindow()
        {
            InitializeComponent();

            _connection = new HubConnectionBuilder().WithUrl("http://localhost:49156/ChatHub").Build();

            _connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _connection.StartAsync();
            };
        }

        private async void connectButton_Click(object sender, RoutedEventArgs e)
        {
            _connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{user}: {message}";
                    MessagesList.Items.Add(newMessage);
                });
            });

            try
            {
                await _connection.StartAsync();
                MessagesList.Items.Add("Connection started");
                ConnectButton.IsEnabled = false;
                SendButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessagesList.Items.Add(ex.Message);
            }
        }

        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _connection.InvokeAsync("SendMessage",
                    UserTextBox.Text, MessageTextBox.Text);
            }
            catch (Exception ex)
            {
                MessagesList.Items.Add(ex.Message);
            }
        }

    }
}
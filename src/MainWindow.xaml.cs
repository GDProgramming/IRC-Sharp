using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

using Meebey.SmartIrc4net;

namespace IRC_Sharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            IRC irc = ((App)Application.Current).Irc; //get irc instance

            string serverUrl = "irc.freenode.net";
            int port = 6667;

            string channel = "#IRCSharpTest";

            irc.Client.OnConnected += (sender, args) =>
            {
                lblConnected.Content = "Connected to " + serverUrl;

                irc.Client.Login("IRCSharp", "IRCSharp", 0, "IrcSharp");
                irc.Client.RfcJoin(channel);

                irc.StartListenerThread();
            };

            irc.Client.OnChannelMessage += new IrcEventHandler(OnChannelMessage);
            irc.Client.OnQueryMessage += new IrcEventHandler(OnQueryMessage);

            try
            {
                irc.Client.Connect(serverUrl, port);
            } catch (Exception e)
            {
                lblConnected.Content = "Error! Failed to connect: " + e.Message;
            }
            
        }

        void OnChannelMessage(object sender, IrcEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => 
            {
                lblMessage.Content = string.Format("{0}:\n({1}) <{2}> {3}", e.Data.Type, e.Data.Channel, e.Data.Nick, @e.Data.Message);
            }));
        }

        void OnQueryMessage(object sender, IrcEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                lblMessage.Content = string.Format("{0}:\n(private) <{1}> {2}", e.Data.Type, e.Data.Nick, @e.Data.Message);
            }));
        }
    }
}

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
        IRC irc;
        string channel = "#IRCSharpTest";
        string nick = "IRCSharp";
        public MainWindow()
        {
            InitializeComponent();

            irc = ((App)Application.Current).Irc; //get irc instance

            string serverUrl = "irc.freenode.net";
            int port = 6667;

            irc.Client.OnConnected += (sender, args) =>
            {
                lblConnected.Content = "Connected to " + serverUrl;

                irc.Client.Login("IRCSharp", "IRCSharp", 0, "IrcSharp");
                irc.Client.RfcJoin(channel);

                //Show we've logged into the channel
                PrimaryTab.Header = "<" + channel + ">";
                UpdateMessageBoxContent("You have successfully logged into " + channel);

                //Start listening to IRC server responses -- DONE ON A BACKGROUND WORKER
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

        protected void SendMessage()
        {
            //Grab the content from the text box
            string msg = txtMessage.Text;
            //Send the message (Allow SmartIrc to handle formatting)
            irc.Client.SendMessage(SendType.Message, channel, msg);
            //Update Textbox content with the message after we format it
            string formattedMsg = string.Format("<{0}> {1}", nick, msg);
            UpdateMessageBoxContent(formattedMsg);

            //Finally, clear the message box content, since we're done sending.
            txtMessage.Clear();
        }

        protected void UpdateMessageBoxContent(string message)
        {
            txtboxChat.Text += Environment.NewLine + message;
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                SendMessage();
        }

        //The following handlers are invoked by the background worker thread, so we need to use Dispatcher to modify the UI.
        //TODO: Reroute to necessary tab, based on the incoming message's channel (e.Data.Channel)
        void OnChannelMessage(object sender, IrcEventArgs e)
        {
            string msg = string.Format("<{0}> {1}", e.Data.Nick, @e.Data.Message);
            Dispatcher.BeginInvoke(new Action(() => 
            {
                UpdateMessageBoxContent(msg);
            }));
        }

        void OnQueryMessage(object sender, IrcEventArgs e)
        {
            string msg = string.Format("(private) <{0}> {1}", e.Data.Nick, @e.Data.Message);
            Dispatcher.BeginInvoke(new Action(() =>
            {
                UpdateMessageBoxContent(msg);
            }));
        }
    }
}

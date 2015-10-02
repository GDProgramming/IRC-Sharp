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
            
            SetupTestConnection();
        }

        /// <summary>
        /// Test method for setting up a connection. Just sits there until disconnected for inactivity See documentation at http://smartirc4net.meebey.net/docs/0.4.0/html/
        /// </summary>
        private void SetupTestConnection()
        {

            //Fetch the IRC connection from our main application.
            IrcConnection irc = ((App)Application.Current).Irc;

            string server = "irc.freenode.net";
            int port = 6667;
            string channel = "#IRCSharpTest";

            //initialise IRC settings
            irc.Connect(server, port);
            irc.WriteLine(Rfc2812.Nick("RFCSharp"), Priority.Critical);
            irc.WriteLine(Rfc2812.User("RFCSharp", 0, "ImaTest"), Priority.Critical);
            irc.WriteLine(Rfc2812.Join(channel));
            irc.Listen(); //Block main thread till we get something back. NOTE: This blocks the UI thread, so no window will show up.
        }
    }
}

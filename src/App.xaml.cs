using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Meebey.SmartIrc4net;

namespace IRC_Sharp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /*
        * If you need something to be globally accessible, instantiate it here.
        * NOTE: Only use this when necessary. This keeps objects in memory. It's useful for the IRC Connection because we will want this accessible at all times.
        */
        private IrcConnection f_Irc;
        public IrcConnection Irc
        {
            get
            {
                if (f_Irc == null)
                {
                    //create new instance
                    f_Irc = new IrcConnection();
                }
                return f_Irc;
            }
        }
        
    }
}

using System.ComponentModel;
using System.Text;
using System.Threading;
using Meebey.SmartIrc4net;

namespace IRC_Sharp
{
    public class IRC
    {
        private IrcClient f_Client;
        private bool f_IsListening = false;

        private BackgroundWorker listenerWorker;

        public IrcClient Client
        {
            get { return f_Client; }
        }

        public bool IsListening
        {
            get { return f_IsListening; }
        }

        public IRC()
        {
            f_Client = new IrcClient();
            f_Client.Encoding = Encoding.UTF8;
            f_Client.SendDelay = 200;
            f_Client.ActiveChannelSyncing = true;

            //Set up our listener worker background thread. This worker will listen for messages from the irc server.
            listenerWorker = new BackgroundWorker();
            listenerWorker.DoWork += (bwSender, bwArgs) =>
            {
                while (f_IsListening)
                {
                    //We use listen once because we need to check to see if we should continue each cycle
                    f_Client.ListenOnce();
                    Thread.Sleep(100);
                }
            };
        }

        public void StartListenerThread()
        {
            f_IsListening = true;
            listenerWorker.RunWorkerAsync();
        }

        public void StopListenerThread()
        {
            if (listenerWorker.IsBusy || f_IsListening)
                f_IsListening = false;
        }

        public void Disconnect()
        {
            StopListenerThread();
            f_Client.Disconnect();
        }
    }
}

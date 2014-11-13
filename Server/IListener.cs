using System;

namespace Server
{
    interface IListener
    {
        event ConnectionEvent userAdded;

        void Start();
        void Stop();
        void UserDisconnected(object sender, Client user);
    }
}
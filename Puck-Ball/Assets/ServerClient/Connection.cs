using System;
using Nakama;
using UnityEngine;

namespace ServerClient
{
    public class Connection
    {
        private API api;
        private Authentication auth;

        public ISocket Socket { get; private set; }

        public void Connect(Action onSuccess)
        {
            var socket = api.Client.NewSocket();
            socket.ConnectAsync(auth.Session, true, 30).ContinueWith(t => {
                Socket = socket;
                onSuccess();
            }, System.Threading.Tasks.TaskContinuationOptions.OnlyOnRanToCompletion).ContinueWith(t => {
                Debug.LogError($"[Connection] Socket couldn't be created {t.Exception.Message}");
            }, System.Threading.Tasks.TaskContinuationOptions.NotOnRanToCompletion);
        }

        public Connection(API api, Authentication auth)
        {
            this.api = api;
            this.auth = auth;
        }
    }
}

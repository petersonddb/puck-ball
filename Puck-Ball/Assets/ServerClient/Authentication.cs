using System;
using UnityEngine;

namespace ServerClient
{
    public class Authentication
    {
        public Nakama.ISession Session { get; private set; }

        private readonly API api;

        public void AuthenticateWithDevice(string deviceId, Action onSuccess, Action<Exception> onFailure = null)
        {
            api.Client.AuthenticateDeviceAsync(deviceId).ContinueWith(t => {
                Session = t.Result;
                onSuccess?.Invoke();
                Debug.Log($"Authenticated with {deviceId} Device ID");
            }, System.Threading.Tasks.TaskContinuationOptions.OnlyOnRanToCompletion).ContinueWith(t => {
                onFailure?.Invoke(t.Exception);
                Debug.Log($"Error authenticating with {deviceId} Device ID: {t.Exception.Message}");
            }, System.Threading.Tasks.TaskContinuationOptions.NotOnRanToCompletion);
        }

        public Authentication(API api)
        {
            this.api = api;
        }
    }
}

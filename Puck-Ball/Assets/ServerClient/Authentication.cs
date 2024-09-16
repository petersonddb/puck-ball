using System;
using UnityEngine;
using System.Threading.Tasks;

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
            }, TaskContinuationOptions.OnlyOnRanToCompletion).ContinueWith(t => {
                onFailure?.Invoke(t.Exception);
                Debug.Log($"Error authenticating with {deviceId} Device ID: {t.Exception.Message}");
            }, TaskContinuationOptions.NotOnRanToCompletion);
        }

        public void AuthenticateWithEmail(string email, string password, Action onSuccess, Action<Exception> onFailure = null)
        {
            api.Client.AuthenticateEmailAsync(email, password).ContinueWith(t => {
                Session = t.Result;
                onSuccess?.Invoke();
                Debug.Log($"Authenticated with {email}");
            }, TaskContinuationOptions.OnlyOnRanToCompletion).ContinueWith(t => {
                onFailure?.Invoke(t.Exception);
                Debug.Log($"Error authenticating with {email}: {t.Exception.Message}");
            }, TaskContinuationOptions.NotOnRanToCompletion);
        }

        public Authentication(API api)
        {
            this.api = api;
        }
    }
}

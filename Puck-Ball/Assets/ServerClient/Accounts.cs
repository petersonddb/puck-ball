using System;
using UnityEngine;

namespace ServerClient
{
    public class Accounts
    {
        private readonly API api;
        private readonly Authentication auth;

        public void GetAccount(Action<global::Accounts.Account> onSuccess, Action<Exception> onFailure)
        {
            api.Client.GetAccountAsync(auth.Session).ContinueWith(t => {
                var account = new global::Accounts.Account
                {
                    Username = t.Result.User.Username,
                    DisplayName = t.Result.User.DisplayName
                };

                onSuccess(account);
            }, System.Threading.Tasks.TaskContinuationOptions.OnlyOnRanToCompletion).ContinueWith(t => {
                onFailure(t.Exception);
            }, System.Threading.Tasks.TaskContinuationOptions.NotOnRanToCompletion);
        }

        public void UpdateAccount(string displayname)
        {
            api.Client.UpdateAccountAsync(auth.Session, displayname, displayname).ContinueWith(t => {
                Debug.LogWarning($"[ServerClient] Couldn't update account with username and displayname {displayname}");
            }, System.Threading.Tasks.TaskContinuationOptions.NotOnRanToCompletion);
        }

        public Accounts(API api, Authentication auth)
        {
            this.api = api;
            this.auth = auth;
        }
    }
}

using System;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using Nakama.TinyJson;
using System.Text;
using Nakama;
using System.Collections.Generic;

namespace ServerClient
{
    public class Matches
    {
        public Nakama.IMatch Match { get; private set; }

        private readonly API api;
        private readonly Authentication auth;
        private readonly Connection conn;

        public void JoinMatch(Action onSuccess) =>
            api.Client.ListMatchesAsync(auth.Session, 1, 100000, 10, true, "puck-ball-match", "").ContinueWith(t => {
                var match = t.Result.Matches.Any();
                if (!match)
                {
                    Debug.LogError($"[Matches] Match NOT created yet in the server!");
                } else {
                    var matchApi = t.Result.Matches.First();
                    conn.Socket.JoinMatchAsync(matchApi.MatchId).ContinueWith(t => {
                        Match = t.Result;
                        onSuccess();
                    }, TaskContinuationOptions.OnlyOnRanToCompletion).ContinueWith(t => {
                        Debug.LogError($"[Matches] Can't Join Match: {t.Exception.Message}");
                    }, TaskContinuationOptions.NotOnRanToCompletion);
                }
            }, TaskContinuationOptions.OnlyOnRanToCompletion).ContinueWith(t => {
                Debug.LogError($"[Matches] Can't List Matches: {t.Exception.Message}");
            }, TaskContinuationOptions.NotOnRanToCompletion);

        public void JoinMatch(string matchId, Action onSuccess) =>
            conn.Socket.JoinMatchAsync(matchId).ContinueWith(t => {
                Match = t.Result;
                onSuccess();
            }, TaskContinuationOptions.OnlyOnRanToCompletion).ContinueWith(t => {
                Debug.LogError($"[Matches] Can't Join Match: {t.Exception.Message}");
            }, TaskContinuationOptions.NotOnRanToCompletion);

        public void SendMatchState(PlayerPosition position) =>
            conn.Socket.SendMatchStateAsync(
                Match.Id, StateOpCode.Position.GetLong(), JsonWriter.ToJson(position)).ContinueWith(t => {
                    Debug.Log("[Matches] Position state sent!");
            }, TaskContinuationOptions.OnlyOnRanToCompletion).ContinueWith(t => {
                Debug.LogWarning("[Matches] Position state couldn't be sent!");
            }, TaskContinuationOptions.NotOnRanToCompletion);

        public void RegisterForStateUpdates(Action<string, PlayerPosition> onPositionUpdate = null) => 
            // TODO: avoid re-registration of the whole switching
            conn.Socket.ReceivedMatchState += matchState => {
                Debug.Log("[Matches] Match state received!");
                switch ((StateOpCode) matchState.OpCode)
                {
                    case StateOpCode.Position:
                        if (onPositionUpdate != null)
                        {
                            var stateJson = Encoding.UTF8.GetString(matchState.State);
                            var positionState = JsonParser.FromJson<PlayerPosition>(stateJson);
                            var userId = matchState.UserPresence.UserId;

                            onPositionUpdate(userId, positionState);
                        }
                        break;
                    default:
                        Debug.LogWarning("Unsupported op code!");
                        break;
                }
            };

        public void RegisterForPresenceUpdates(Action<IList<string>, IList<string>> onPresencesUpdate) => 
            conn.Socket.ReceivedMatchPresence += matchPresenceEvent => {
                onPresencesUpdate(
                    matchPresenceEvent.Joins.Select(j => j.UserId).ToList(),
                    matchPresenceEvent.Leaves.Select(l => l.UserId).ToList());
            };

        public Matches(API api, Authentication auth, Connection conn)
        {
            this.api = api;
            this.auth = auth;
            this.conn = conn;
        }
    }

    enum StateOpCode : long
    {
        Position = 0
    }

    static class StateOpCodeMethods
    {
        public static long GetLong(this StateOpCode op)
        {
            return (long) op;
        }
    }
}

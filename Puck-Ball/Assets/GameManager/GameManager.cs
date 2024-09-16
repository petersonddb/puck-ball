using System.Collections.Generic;
using System.Linq;
using ServerClient;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerPerfab;
    public Material NonPlayablePlayerMaterial;
    private Dictionary<string, GameObject> players;
    private Matches matches;
    private IList<PresenceGameUpdate> PresenceGameUpdates;
    private IList<PositionGameUpdate> PositionGameUpdates;

    void Awake()
    {
        PresenceGameUpdates = new List<PresenceGameUpdate>();
        PositionGameUpdates = new List<PositionGameUpdate>();
        players = new Dictionary<string, GameObject>();
        var sm = ServerManager.Instance;
        API api = sm.Api;
        Authentication auth = sm.Auth;

        Connection conn = new(api, auth);
        conn.Connect(() => {
            sm.Conn = conn;
            Matches matches = new(api, auth, conn);
            matches.RegisterForStateUpdates(UpdatePlayerPosition);
            matches.RegisterForPresenceUpdates(UpdateJoins);
            matches.JoinMatch("3010ac6a-9042-47e0-8437-c520c650ab68.nakama-node-1", () => {
                Debug.Log($"Joined Match {matches.Match.Id}");

                sm.Mats = matches;
                this.matches = matches;

                UpdateJoins(matches.Match.Presences.Select(p => p.UserId).ToList());
            });
        });
    }

    void Update()
    {
        SpawnPlayers();
        PositionPlayers();
    }

    private void SpawnPlayers()
    {
        IList<string> joinsIds, leavesIds;

        lock (PresenceGameUpdates)
        {
            joinsIds = PresenceGameUpdates.Where(u => u.Joining).Select(u => u.UserId).ToList();
            leavesIds = PresenceGameUpdates.Where(u => !u.Joining).Select(u => u.UserId).ToList();

            PresenceGameUpdates.Clear();
        }

        if (joinsIds != null)
        {
            foreach (var userId in joinsIds)
            {
                var go = Instantiate(PlayerPerfab);

                if (ServerManager.Instance.Auth.Session.UserId != userId)
                {
                    Destroy(go.GetComponent<PlayerController>());
                    go.GetComponentInChildren<Renderer>().material = NonPlayablePlayerMaterial;
                }

                players.Add(userId, go);
            }
        }

        if (leavesIds != null)
        {
            foreach (var userId in leavesIds)
            {
                Destroy(players[userId]);
                players.Remove(userId);
            }
        }
    }

    private void PositionPlayers()
    {
        lock (PositionGameUpdates)
        {
            foreach (var update in PositionGameUpdates)
            {
                var userId = update.UserId;

                if (players.ContainsKey(userId) && userId != ServerManager.Instance.Auth.Session.UserId)
                {
                    players[userId].transform.position = update.Position.ToVector();
                }
            }

            PositionGameUpdates.Clear();
        }
    }

    private void UpdatePlayerPosition(string userId, PlayerPosition position)
    {
        lock (PositionGameUpdates)
        {
            PositionGameUpdates.Add(
                new PositionGameUpdate { UserId = userId, Position = position });
        }
    }

    private void UpdateJoins(IList<string> joinsIds = null, IList<string> leavesIds = null)
    {
        if (joinsIds != null)
        {   
            foreach (var userId in joinsIds)
            {
                lock (PresenceGameUpdates)
                {
                    PresenceGameUpdates.Add(
                        new PresenceGameUpdate { UserId = userId, Joining = true });
                }
            }
        }

        if (leavesIds != null)
        {
            foreach (var userId in leavesIds)
            {
                lock (PresenceGameUpdates)
                {
                    PresenceGameUpdates.Add(
                        new PresenceGameUpdate { UserId = userId, Joining = false });
                }
            }
        }
    }

    private class PresenceGameUpdate
    {
        public string UserId { get; set; }
        public bool Joining { get; set; }
    }
    
    private class PositionGameUpdate
    {
        public string UserId { get; set; }
        public PlayerPosition Position { get; set; }
    }
}

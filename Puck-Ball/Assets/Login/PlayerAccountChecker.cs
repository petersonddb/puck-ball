using ServerClient;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAccountChecker : MonoBehaviour
{
    private API api;
    private Authentication auth;
    private bool isKnown;

    void Awake()
    {
        // If the user's device ID is already stored, grab that - alternatively get the System's unique device identifier.
        var deviceId = PlayerPrefs.GetString("deviceId", SystemInfo.deviceUniqueIdentifier);

        // If the device identifier is invalid then let's generate a unique one.
        if (deviceId == SystemInfo.unsupportedIdentifier)
        {
            deviceId = System.Guid.NewGuid().ToString();
        }

        // Save the user's device ID to PlayerPrefs so it can be retrieved during a later play session for re-authenticating.
        PlayerPrefs.SetString("deviceId", deviceId);

        api = new();
        auth = new(api);
        auth.AuthenticateWithDevice(deviceId, () => {
            var accounts = new ServerClient.Accounts(api, auth);
            accounts.GetAccount(account => {
                if (account.DisplayName != null)
                {
                    isKnown = true;
                }
            }, ex => {
                Debug.LogWarning("[PlayerAccountChecker] Couldn't retrieve account");
            });
        });
    }

    void Update()
    {
        if (isKnown)
        {
            SceneManager.LoadScene("Stadium", LoadSceneMode.Single);
        }
    }
}

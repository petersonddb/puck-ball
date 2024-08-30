using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginButton : MonoBehaviour
{
    public GameObject NameInput;

    private TMP_InputField nameInputComponent;
    
    private bool isUpdated;

    void Start()
    {
        nameInputComponent = NameInput.GetComponent<TMP_InputField>();
    }

    void Update()
    {
        if (isUpdated)
        {
            SceneManager.LoadScene("Stadium");
        }
    }

    public void Authenticate()
    {
        var deviceId = PlayerPrefs.GetString("deviceId");
        string displayname = nameInputComponent.text;

        var api = new ServerClient.API();
        var auth = new ServerClient.Authentication(api);
        auth.AuthenticateWithDevice(deviceId, () => {
            var accounts = new ServerClient.Accounts(api, auth);
            accounts.UpdateAccount(displayname);
            isUpdated = true;
        });
    }
}

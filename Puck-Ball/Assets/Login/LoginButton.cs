using System.Linq;
using ServerClient;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginButton : MonoBehaviour
{
    public GameObject EmailInput;
    public GameObject PasswordInput;

    private bool isUpdated;

    void Start()
    {
        var email = PlayerPrefs.GetString("email");
        if (email != null)
        {
            EmailInput.GetComponent<TMP_InputField>().text = email;
        }

        ServerManager.Initialize(); // TODO: Move to its own object!
        ServerManager.Instance.Api = new API();
    }

    void Update()
    {
        if (isUpdated)
        {
            PlayerPrefs.SetString(
                "email", EmailInput.GetComponent<TMP_InputField>().text);
            SceneManager.LoadScene("Stadium");
        }
    }

    public void Authenticate()
    {
        var email = EmailInput.GetComponent<TMP_InputField>().text;
        var pass = PasswordInput.GetComponent<TMP_InputField>().text;

        var api = ServerManager.Instance.Api;
        var auth = new Authentication(api);
        auth.AuthenticateWithEmail(email, pass, () => {
            ServerManager.Instance.Auth = auth;
            
            var accounts = new ServerClient.Accounts(api, auth);
            string displayname = email.Split("@")?.First();
            accounts.UpdateAccount(displayname);
            isUpdated = true;
        });
    }
}

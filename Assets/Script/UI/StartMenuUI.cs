using Assets.Script.Network;
using Assets.Script.Player;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuUI : MonoBehaviour
{
    // Start is called before the first frame update
    public static StartMenuUI instanceUI;
    public GameObject UsernameUI;
    public GameObject TeamsitesUI;
    public InputField usernameField;
    public Text errorUsername;
    public Text errorOnline;

    private void Awake()
    {
        instanceUI = this;
    }

    public bool IsUsernameVaild()
    {
        string username = usernameField.text;

        if (Regex.IsMatch(username, @"^[a-zA-Z0-9]+$"))
        {
            Clients newPlayer = new Clients();
            newPlayer.username = username;
            newPlayer.isLocalPlayer = true;
            NetworkManager.playerList.Add(username, newPlayer);
            return true;
        }
        else
        {
            errorUsername.enabled = true;
            return false;

        }
    }

    public void NoConnection()
    {
        errorOnline.enabled = true;
    }

    public void ClearAllErrors()
    {
        errorUsername.enabled = false;
        errorOnline.enabled = false;
    }

    public void enableClient()
    {
        TeamsitesUI.SetActive(false);
        ClientCameraUI.instancePlayerUI.enableClientCamera();
    }

    public void RespawnAndGetTeamsites()
    {
        TeamsitesUI.SetActive(true);
        Cursor.visible = true;
    }
}

using UnityEngine;


public class PauseMenu_UI : MonoBehaviour
{
    public GameObject pauseMenu;
    public PlayerMovement playerMovement;
    bool activeMenu = false;
    bool activeMovement = true;

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (activeMenu == false)
            {
                Cursor.visible = true;
                activeMenu = true;
                activeMovement = false;
            }
            else
            {
                Cursor.visible = false;
                activeMenu = false;
                activeMovement = true;
            }
            pauseMenu.SetActive(activeMenu);
            playerMovement.enabled = activeMovement;
        }
    }

    //If the Local Player chooses to leave the Server
    public void DisconnectFromServer()
    {
        NetworkManager.instance.DisconnectFromServerString();
        Application.Quit();
    }
}

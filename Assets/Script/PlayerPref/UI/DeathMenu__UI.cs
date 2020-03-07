using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenu__UI : MonoBehaviour
{
    public GameObject deathMenu;
    public GameObject Player;
    public void getTeamSites()
    {
        Destroy(Player);
        deathMenu.SetActive(false);
        StartMenuUI.instanceUI.RespawnAndGetTeamsites();
    }
}

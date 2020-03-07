using Assets.Script;
using Assets.Script.Player;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    /*
     * This class cares about the Health of the Player and Damage System
     * 
     * - Lava does 40 Damage per Second
     */
    public GameObject Player;
    public float Health = 100f;
    private float lavaDamage = 40f;
    private string damageCause = "";
    private float cooldown = 0f;
    private int lavaCooldown = 5;
    private float startCooldownTime = 0f;
    private float deltaTimePassed = 0f;
    public GameObject DeathMenu;
    public PlayerMovement playerMovement;
    public static PlayerHealth instanceHealth;

    private void Awake()
    {
        instanceHealth = this;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Lava")
        {
            Debug.Log("Enter Lava");
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Lava")
        {
            damageCause = "Lava";
            if (cooldown <= 0)
            {

                TakeDamage(lavaDamage, damageCause);

                Debug.Log("Player has been hitted through LAVA!");

                cooldown = lavaCooldown
                    ;
                startCooldownTime = Time.time;

            }
            else if (cooldown > 0)
            {
                deltaTimePassed = 0;
                deltaTimePassed = Time.time - startCooldownTime;
                damageCause = "Lava";
                if (deltaTimePassed > cooldown)
                {
                    TakeDamage(lavaDamage, damageCause);

                    Debug.Log("Player has been hitted through LAVA!");
                    cooldown = lavaCooldown;
                    startCooldownTime = Time.time;
                }

                Debug.Log("Kein schaden bis " + deltaTimePassed);
            }

        }
    }

    public void TakeDamage(float damageAmount, string damageCause)
    {
        Debug.Log(damageAmount + damageCause);
        if (NetworkManager.playerList[Player.name].isLocalPlayer)
        {
            Health -= damageAmount;
            if (Health <= 0)
            {
                DeathMenu.SetActive(true);
                playerMovement.enabled = false;
                Cursor.visible = true;
                NetworkManager.instance.localPlayerDied(damageCause);
                MeshRenderer[] visiblePlayer = Player.GetComponentsInChildren<MeshRenderer>();
                foreach (var item in visiblePlayer)
                {
                    item.enabled = false;
                }
            }
        }
        else
        {
            NetworkManager.instance.OtherPlayerDamagedHasToBeSend(damageAmount, NetworkManager.playerList[Player.name].username);
        }
    }
}



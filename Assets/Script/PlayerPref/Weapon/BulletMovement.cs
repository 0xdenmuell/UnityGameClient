using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private Camera cam;
    public GameObject Player;
    private float Speed = 100f;
    public Rigidbody BulletRb;
    private float maxTimeLive = 2.0f;
    private float bulletDamage = 50f;
    private string damageType = "Rocket";
    private void Start()
    {
        Player = BulletRb.transform.root.gameObject;
        cam = Player.GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        maxTimeLive -= Time.deltaTime;
        Debug.Log(maxTimeLive);
        if (maxTimeLive <= 0.0f)
        {
            Destroy(BulletRb.gameObject);
            Destroy(BulletRb.GetComponent<GameObject>());
        }
    }
    void FixedUpdate()
    {
        float mag = Player.GetComponent<Rigidbody>().velocity.magnitude;
        if (mag >= 1 || mag <=-1)
        {
            BulletRb.AddForce(transform.TransformDirection(Vector3.forward * Speed * mag));
        }
        else
        {
            BulletRb.AddForce(transform.TransformDirection(Vector3.forward * Speed));
        }


    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.name != Player.name)
        {
            GameObject hittedPlayer = collision.gameObject;
            PlayerHealth playerHealthScript =  hittedPlayer.GetComponent<PlayerHealth>();
            playerHealthScript.TakeDamage(bulletDamage, damageType);
            }
        Destroy(BulletRb.gameObject);
        Destroy(BulletRb.GetComponent<GameObject>());
    }
}

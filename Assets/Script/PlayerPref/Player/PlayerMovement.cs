using Assets.Script;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    public GameObject player;
    public Rigidbody playerRb;
    public Transform bulletPrefab;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;
    public float speedMovement = 200f;
    public float gravity = 100f;
    public float jumpgHight = 20f;

    private float maxVelocity = 30f;
    private float speed = 20f;
    private float lookSensivity = 500f;
    private float _xRot = 0;
    private float _yRot = 0;
    private float min_rotation_down = 90;
    private float max_rotation_up = 270;
    private Vector3 grav;
    public void Start()
    {
        //Set Cursor to not be visible
        Cursor.visible = false;
        grav = new Vector3(0.0f, -gravity, 0.0f);
        // Transform bullet = Instantiate(bulletPrefab) as Transform;
        // Physics.IgnoreCollision(bullet.GetComponentInChildren<Collider>(), GetComponentInChildren<Collider>());
    }
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void RotateCamera(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;
    }

    private void FixedUpdate()
    {
        /*
        float mHorizontal = Input.GetAxisRaw("Horizontal");
        float mVertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(mHorizontal, 0.0f, mVertical);
        playerRb.AddForce(movement);
        */
        //Gravity();
        PerfomMovement();
    }
    private void Gravity()
    {
        playerRb.AddForce(grav);
    }
    private void PerfomMovement()
    {

        float zero = 0.0f;
        //Position
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space");
            playerRb.AddForce(new Vector3(0.0f, 20f, 0.0f), ForceMode.Impulse);
        }
        if (velocity != Vector3.zero && Input.GetAxisRaw("Vertical") > zero) //moving forward
        {
            Debug.Log("forward");
            if (playerRb.velocity.magnitude < maxVelocity)
            {
                Vector3 rotationVector = new Vector3(cam.transform.rotation.x, 0.0f, cam.transform.rotation.z);

                playerRb.AddForce(new Vector3(cam.transform.TransformDirection(Vector3.forward * speedMovement).x, 0.0f, cam.transform.TransformDirection(Vector3.forward * speedMovement).z));

            }
            else
            {
                playerRb.velocity = playerRb.velocity.normalized * maxVelocity;
            }

        }
        if (velocity != Vector3.zero && Input.GetAxisRaw("Vertical") < zero) //moving backwards
        {
            Debug.Log("backwards");
            if (playerRb.velocity.magnitude < maxVelocity)
            {
                Vector3 rotationVector = new Vector3(cam.transform.rotation.x, 0.0f, cam.transform.rotation.z);
                playerRb.AddForce(cam.transform.TransformDirection(-1 * Vector3.forward * speedMovement));
            }
            else
            {
                playerRb.velocity = playerRb.velocity.normalized * maxVelocity;
            }
        }
        if (velocity != Vector3.zero && Input.GetAxisRaw("Horizontal") > zero) //moving right
        {
            Debug.Log("right");
            if (playerRb.velocity.magnitude < maxVelocity)
            {
                Vector3 rotationVector = new Vector3(cam.transform.rotation.x, 0.0f, cam.transform.rotation.z);
                playerRb.AddForce(cam.transform.TransformDirection(Vector3.right * speedMovement));
            }
            else
            {
                playerRb.velocity = playerRb.velocity.normalized * maxVelocity;
            }
        }
        if (velocity != Vector3.zero && Input.GetAxisRaw("Horizontal") < zero) //moving left
        {
            Debug.Log("left");
            if (playerRb.velocity.magnitude < maxVelocity)
            {
                Vector3 rotationVector = new Vector3(cam.transform.rotation.x, 0.0f, cam.transform.rotation.z);
                playerRb.AddForce(cam.transform.TransformDirection(Vector3.left * speedMovement));
            }
            else
            {
                playerRb.velocity = playerRb.velocity.normalized * maxVelocity;
            }
        }
        //Rotation
        if (rotation != Vector3.zero)
        {
            playerRb.MoveRotation(playerRb.rotation * Quaternion.Euler(rotation));
        }

        Debug.Log(speedMovement);

        string sendPositionPlayer = player.transform.position.x + "|" + player.transform.position.y + "|" + player.transform.position.z;
        string sendPlayerRotation = player.transform.eulerAngles.x + "|" + player.transform.eulerAngles.y + "|" + player.transform.eulerAngles.z;
        string sendCameraRotation = cam.transform.eulerAngles.x + "|" + cam.transform.eulerAngles.y + "|" + cam.transform.eulerAngles.z;

        string serverMessage = player.name + ":" + sendPositionPlayer + ":" + sendPlayerRotation + ":" + sendCameraRotation;

        //Send Position and Rotation to the Server
        DataSender.SendPositionClientToServer(serverMessage);

    }

    void Update()
    {
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov * Time.deltaTime;
        Vector3 _movVertical = transform.forward * _zMov * Time.deltaTime;

        Vector3 _velocity = (transform.right * _xMov + transform.forward * _zMov) * speed;

        Move(_velocity);

        _yRot += Input.GetAxisRaw("Mouse X") * lookSensivity * Time.fixedDeltaTime;
        _xRot += Input.GetAxisRaw("Mouse Y") * lookSensivity * Time.fixedDeltaTime;

        _xRot = Mathf.Clamp(_xRot, -90f, 90f);

        cam.transform.rotation = Quaternion.Euler(-_xRot, _yRot, 0f);
        //cam.transform.rotation = Quaternion.Euler(-_xRot, 0f, 0f);

        Rotate(new Vector3(0f, cam.transform.rotation.y, 0f));
        RotateCamera(new Vector3(-_xRot, 0f, 0f));
        //cam.transform.rotation = Quaternion.Euler(-_xRot, _yRot, 0f);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("Ground");
            speedMovement = 40;
        }
        else
        {
            Debug.Log("Not Ground");
            speedMovement = 10;
        }
    }
}

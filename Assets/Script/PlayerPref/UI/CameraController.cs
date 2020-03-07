using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public GameObject _player;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z);
    }
}

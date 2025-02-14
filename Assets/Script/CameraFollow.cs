using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        Vector3 targetPos = player.position + offset;
        targetPos.y = 0; // Lock vertical position
        transform.position = targetPos;
    }
}
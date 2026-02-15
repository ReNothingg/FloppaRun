using UnityEngine;

public class SmoothFollow2D : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    public float ofssetX;
    public float ofssetY;
    public float ofssetZ;

    void FixedUpdate()
    {
        Vector3 desiredPosition =  new Vector3(player.position.x + ofssetX, player.position.y + ofssetY, -10) + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, ofssetZ);
    }
}
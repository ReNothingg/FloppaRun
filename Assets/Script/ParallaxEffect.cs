using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxEffectMultiplier;
    private Vector3 lastCameraPosition;
    void Start() { lastCameraPosition = cameraTransform.position; }

    void Update() {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += deltaMovement * parallaxEffectMultiplier;
        lastCameraPosition = cameraTransform.position;
    }
}
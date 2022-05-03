using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public Transform target;
    public Vector3 offset;
    [Range(1, 10)]
    public float smoothFactor;

    public void FixedUpdate()
    {
        follow();
    }

    void follow()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor*Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
}

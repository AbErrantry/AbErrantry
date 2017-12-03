using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; //that which is being followed
    public float smoothSpeed; //the higher this value is, the faster we lock on to the target
    public Vector3 offset;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed*Time.deltaTime);
        transform.position = smoothedPosition;
    }
}

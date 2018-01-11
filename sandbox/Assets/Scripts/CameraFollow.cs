using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Bounds currentBounds;

    private float alignDuration = 1f;
    public Transform target; //that which is being followed
    public float smoothSpeed; //the higher this value is, the faster we lock on to the target
    public Vector3 offset;
    float targX;
    float targY;

    IEnumerator AlignToNewBounds()
    {

        Vector3 startVect = transform.position;
        Vector3 trackingVect = transform.position;

        targX = currentBounds.center.x;
        targY = currentBounds.center.y;
        Vector3 targetPosition = new Vector3(targX, targY, transform.position.z);

        float lerpTime = 0;
        while (lerpTime < alignDuration)
        {

            lerpTime += Time.deltaTime;
            trackingVect = Vec3Lerp(lerpTime, alignDuration, startVect, targetPosition);
            transform.position = trackingVect;
            yield return 0;
        }

        transform.position = targetPosition;
    }

    public void SetNewBounds(Bounds newBounds)
    {

        currentBounds = newBounds;
        StartCoroutine(AlignToNewBounds());
    }

    public static Vector3 Vec3Lerp(float currentTime, float duration, Vector3 v3_start, Vector3 v3_target)
    {
        float step = (currentTime / duration);
        Vector3 v3_ret = Vector3.Lerp(v3_start, v3_target, step);
        return v3_ret;
    }

    private void LateUpdate()
    {
        Vector3 temp = new Vector3(target.position.x, 0, target.position.z);
        Vector3 desiredPosition = temp + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}

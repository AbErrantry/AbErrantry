using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraShift : MonoBehaviour 
{
    public CinemachineVirtualCamera vcam;
    public Transform player;

    private Camera cam;
    private CinemachineFramingTransposer body;
    
    private float shiftedLeftX;
    private float shiftedRightX;
    private float unshiftedX;

    private float shiftedY;
    private float unshiftedY;

    private float deadZoneWidth;
    private float deadZoneHeight;

    private void Start()
    {
        cam = GetComponent<Camera>();
        body = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();

        shiftedY = 0.50f;
        shiftedLeftX = 0.875f;
        shiftedRightX = 0.125f;

        unshiftedX = body.m_ScreenX;
        unshiftedY = body.m_ScreenY;

        deadZoneHeight = body.m_DeadZoneHeight;
        deadZoneWidth = body.m_DeadZoneWidth;
    }

    //left is true, right is false
    public bool ShiftCameraLeft()
    {
        bool result;
        Vector3 screenPos = cam.WorldToScreenPoint(player.position);
        body.m_ScreenY = shiftedY;
        body.m_DeadZoneHeight = 0.0f;
        body.m_DeadZoneWidth = 0.0f;
        if(screenPos.x <= Screen.width / 2)
        {
            body.m_ScreenX = shiftedRightX;
            result = false;
        }
        else
        {
            body.m_ScreenX = shiftedLeftX;
            result = true;
        }
        StopAllCoroutines();
        StartCoroutine(CameraLerp(6f, 2f));
        return result;
    }

    public void ResetCamera()
    {
        body.m_ScreenX = unshiftedX;
        body.m_ScreenY = unshiftedY;
        body.m_DeadZoneHeight = deadZoneHeight;
        body.m_DeadZoneWidth = deadZoneWidth;
        StopAllCoroutines();
        StartCoroutine(CameraLerp(6f, 5f));
    }

    private IEnumerator CameraLerp(float lerpTime, float endSize)
    {
        float lerpStart = Time.time;
        while(Time.time - lerpStart < lerpTime)
        {
            vcam.m_Lens.OrthographicSize = Mathf.Lerp(vcam.m_Lens.OrthographicSize, endSize, (Time.time - lerpStart) / lerpTime);
            yield return null;
        }
    }
}

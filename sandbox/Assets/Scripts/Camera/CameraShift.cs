using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraShift : MonoBehaviour 
{
    public CinemachineVirtualCamera vcam;
    public Transform player;

    private Camera cam;
    private CinemachineFramingTransposer body;
    
    private float shiftedLeftX_75;
    private float shiftedRightX_75;
    private float shiftedLeftX_50;
    private float shiftedRightX_50;
    private float unshiftedX;

    private float shiftedY;
    private float unshiftedY;

    private float xDamping;
    private float yDamping;

    private float unshiftedOrthSize;
    private float shiftedOrthSize;

    private float deadZoneWidth;
    private float deadZoneHeight;

    private float shiftTime;

    private void Start()
    {
        cam = GetComponent<Camera>();
        body = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();

        shiftedY = 0.50f;

        shiftedLeftX_75 = 0.875f;
        shiftedLeftX_50 = 0.75f;

        shiftedRightX_75 = 0.125f;
        shiftedRightX_50 = 0.25f;

        shiftedOrthSize = 2.0f;

        unshiftedOrthSize = vcam.m_Lens.OrthographicSize;

        unshiftedX = body.m_ScreenX;
        unshiftedY = body.m_ScreenY;

        xDamping = body.m_XDamping;
        yDamping = body.m_YDamping;

        deadZoneHeight = body.m_DeadZoneHeight;
        deadZoneWidth = body.m_DeadZoneWidth;

        shiftTime = 3.0f;
    }

    //left is true, right is false
    public bool ShiftCameraLeft(bool is75)
    {
        bool result;
        Vector3 screenPos = cam.WorldToScreenPoint(player.position);

        body.m_ScreenY = shiftedY;

        body.m_DeadZoneHeight = 0.0f;
        body.m_DeadZoneWidth = 0.0f;
        
        body.m_XDamping = 0.0f;
        body.m_YDamping = 0.0f;

        float shiftedX = 0.0f;

        if(screenPos.x <= Screen.width / 2)
        {
            if(is75)
            {
                shiftedX = shiftedRightX_75;
            }
            else
            {
                shiftedX = shiftedRightX_50;
            }
            result = false;
        }
        else
        {
            if(is75)
            {
                shiftedX = shiftedLeftX_75;
            }
            else
            {
                shiftedX = shiftedLeftX_50;
            }
            result = true;
        }

        StopAllCoroutines();
        StartCoroutine(CameraLerp(shiftTime, shiftedX, shiftedOrthSize));

        return result;
    }

    public void ResetCamera()
    {
        body.m_ScreenY = unshiftedY;

        body.m_DeadZoneHeight = deadZoneHeight;
        body.m_DeadZoneWidth = deadZoneWidth;

        body.m_XDamping = xDamping;
        body.m_YDamping = yDamping;

        StopAllCoroutines();
        StartCoroutine(CameraLerp(shiftTime, unshiftedX, unshiftedOrthSize));
    }

    private IEnumerator CameraLerp(float lerpTime, float endLoc, float endSize)
    {
        float lerpStart = Time.time;
        while(Time.time - lerpStart < lerpTime)
        {
            float time = (Time.time - lerpStart) / lerpTime;
            vcam.m_Lens.OrthographicSize = Mathf.SmoothStep(vcam.m_Lens.OrthographicSize, endSize, time);
            body.m_ScreenX = Mathf.SmoothStep(body.m_ScreenX, endLoc, time);
            yield return null;
        }
    }
}

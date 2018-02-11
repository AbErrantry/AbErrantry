using Cinemachine;
using System.Collections;
using UnityEngine;

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

    private float xDampingUnshifted;
    private float yDampingUnshifted;
    private float xDampingShifted;
    private float yDampingShifted;

    private float unshiftedOrthSize;
    private float shiftedOrthSize;

    private float deadZoneWidthUnshifted;
    private float deadZoneHeightUnshifted;
    private float deadZoneWidthShifted;
    private float deadZoneHeightShifted;

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

        xDampingUnshifted = body.m_XDamping;
        yDampingUnshifted = body.m_YDamping;

        xDampingShifted = 0.0f;
        yDampingShifted = 0.0f;

        deadZoneHeightUnshifted = body.m_DeadZoneHeight;
        deadZoneWidthUnshifted = body.m_DeadZoneWidth;
        deadZoneHeightShifted = 0.0f;
        deadZoneWidthShifted = 0.0f;

        shiftTime = 3.0f;
    }

    //left is true, right is false
    public bool ShiftCameraLeft(bool is75)
    {
        bool result;
        Vector3 screenPos = cam.WorldToScreenPoint(player.position);
        body.m_XDamping = xDampingShifted;
        body.m_YDamping = yDampingShifted;
        float shiftedX = 0.0f;

        if (screenPos.x <= Screen.width / 2)
        {
            if (is75)
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
            if (is75)
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
        StartCoroutine(CameraLerp(shiftTime, shiftedX, shiftedY, deadZoneHeightShifted, deadZoneWidthShifted, shiftedOrthSize));
        return result;
    }

    public void ResetCamera()
    {
        body.m_XDamping = xDampingUnshifted;
        body.m_YDamping = yDampingUnshifted;
        StopAllCoroutines();
        StartCoroutine(CameraLerp(shiftTime, unshiftedX, unshiftedY, deadZoneHeightUnshifted, deadZoneWidthUnshifted, unshiftedOrthSize));
    }

    private IEnumerator CameraLerp(float lerpTime, float endLocX, float endLocY, float endDeadZoneHeight, float endDeadZoneWidth, float endSize)
    {
        float lerpStart = Time.time;
        while (Time.time - lerpStart < lerpTime)
        {
            float time = (Time.time - lerpStart)/ lerpTime;
            vcam.m_Lens.OrthographicSize = Mathf.SmoothStep(vcam.m_Lens.OrthographicSize, endSize, time);
            body.m_ScreenX = Mathf.SmoothStep(body.m_ScreenX, endLocX, time);
            body.m_ScreenY = Mathf.SmoothStep(body.m_ScreenY, endLocY, time);
            body.m_DeadZoneHeight = Mathf.SmoothStep(body.m_DeadZoneHeight, endDeadZoneHeight, time);
            body.m_DeadZoneWidth = Mathf.SmoothStep(body.m_DeadZoneWidth, endDeadZoneWidth, time);
            yield return null;
        }
    }
}

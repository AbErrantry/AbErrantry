using UnityEngine;
using Cinemachine;
using Character2D;

public class CameraShift : MonoBehaviour {

    public CinemachineVirtualCamera cineCamera;
    public PlayerInput player;
    public float shiftedX;
    public float unshiftedX;

    private void LateUpdate()
    {
        if (!player.acceptInput)
        {
            cineCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = shiftedX;
        }
        else
        {
            cineCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = unshiftedX;
        }
    }
}

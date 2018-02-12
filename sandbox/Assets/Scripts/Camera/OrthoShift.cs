using System.Collections;
using UnityEngine;

public class OrthoShift : MonoBehaviour
{
	public CameraShift cameraShift;

	[Tooltip("How much should the orthographic zoom in/out be. (1 min, 25 max")]
	[Range(1.0f, 25.0f)]
	public float shiftAmount;

	[Tooltip("How many seconds should the orthographic zoom in/out be. (1s min, 30s max")]
	[Range(1.0f, 30.0f)]
	public float shiftTime;
	public void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log("Entered");
		if (collision.gameObject.name == "Knight")
		{
			cameraShift.SetUnshiftedOrthSize(shiftAmount);
			cameraShift.OrthoZoom(shiftTime, shiftAmount);
		}
	}

	public void OnTriggerExit2D(Collider2D collision)
	{
		//Debug.Log("Exited");

		if (collision.gameObject.name == "Knight")
		{
			cameraShift.SetUnshiftedOrthSize(5);
			cameraShift.OrthoZoom(shiftTime, 5);
		}
	}
}

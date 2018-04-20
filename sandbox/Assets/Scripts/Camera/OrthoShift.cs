using System.Collections;
using UnityEngine;

public class OrthoShift : MonoBehaviour
{
	public CameraShift camShift;
	public float shiftSpeed;
	[Tooltip("How much should the orthographic zoom in/out be. (1 min, 25 max")]
	[Range(1.0f, 25.0f)]
	public float shiftAmount;

	public void Start()
	{
		camShift = GameObject.Find("Main Camera").GetComponent<CameraShift>();
	}
	public void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log("Entered");
		if (collision.gameObject.name == "Knight")
		{
			camShift.SetUnshiftedOrthSize(shiftAmount);
			camShift.OrthoZoom(shiftAmount, shiftSpeed);
		}
	}

	public void OnTriggerExit2D(Collider2D collision)
	{
		//Debug.Log("Exited");

		if (collision.gameObject.name == "Knight")
		{
			camShift.SetUnshiftedOrthSize(5);
			camShift.OrthoZoom(5, shiftSpeed);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
	public float speed;
	public bool requiresInput;

	private float maxSpeedMultiplier;
	private float minSpeedMultiplier;

	public Vector3 startLoc;
	public Vector3 endLoc;

	private bool moveToEnd;
	private bool isMoving;
	private bool isDelayed;

	private bool directedTowardsEnd;
	private bool directedTowardsStart;

	public List<GameObject> children;

	private Rigidbody2D rb;

	private Vector3 velocity;
	private Vector3 lastPosition;

	private void Start()
	{
		velocity = new Vector3();
		lastPosition = transform.position;

		rb = GetComponent<Rigidbody2D>();

		transform.position = startLoc;
		moveToEnd = true;
		if (requiresInput)
		{
			isMoving = false;
		}
		else
		{
			isMoving = true;
		}
		isDelayed = false;

		directedTowardsEnd = false;
		directedTowardsStart = false;

		maxSpeedMultiplier = 1.0f;
		minSpeedMultiplier = 0.2f;
	}

	private void LateUpdate()
	{
		if (isMoving && !isDelayed)
		{
			if (moveToEnd)
			{
				if (Vector3.Distance(transform.position, endLoc) < 0.01f)
				{
					if (!directedTowardsEnd)
					{
						moveToEnd = false;
						isDelayed = true;
						StartCoroutine(DelayAtTarget());
					}
				}
				else
				{
					//rb.MovePosition(transform.position + (endLoc - transform.position).normalized * speed * GetSpeedMultiplier() * Time.deltaTime);
					transform.Translate((endLoc - transform.position).normalized * speed * GetSpeedMultiplier() * Time.deltaTime);
				}
			}
			else
			{
				if (Vector3.Distance(transform.position, startLoc) < 0.01f)
				{
					if (!directedTowardsStart)
					{
						moveToEnd = true;
						isDelayed = true;
						StartCoroutine(DelayAtTarget());
					}
				}
				else
				{
					//rb.MovePosition(transform.position + (startLoc - transform.position).normalized * speed * GetSpeedMultiplier() * Time.deltaTime);
					transform.Translate((startLoc - transform.position).normalized * speed * GetSpeedMultiplier() * Time.deltaTime);
				}
			}
		}
		velocity = (transform.position - lastPosition) / Time.deltaTime;
		lastPosition = transform.position;

		foreach (GameObject obj in children)
		{
			if (obj.GetComponent<Character2D.CharacterMovement>())
			{
				if (!obj.GetComponent<Character2D.CharacterMovement>().isJumping)
				{
					obj.GetComponent<Rigidbody2D>().velocity = new Vector2(obj.GetComponent<Rigidbody2D>().velocity.x + velocity.x, velocity.y);
				}
			}
			else
			{
				obj.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y);
			}
		}
	}

	private float GetSpeedMultiplier()
	{
		float result;
		if (Vector3.Distance(transform.position, endLoc) < maxSpeedMultiplier)
		{
			result = Vector3.Distance(transform.position, endLoc);
			if (result < minSpeedMultiplier)
			{
				result = minSpeedMultiplier;
			}
		}
		else if (Vector3.Distance(transform.position, startLoc) < maxSpeedMultiplier)
		{
			result = Vector3.Distance(transform.position, startLoc);
			if (result < minSpeedMultiplier)
			{
				result = minSpeedMultiplier;
			}
		}
		else
		{
			result = maxSpeedMultiplier;
		}
		return result;
	}

	public void SetMovement(bool invokedMove, bool isDirectional = false, bool isTowardsEnd = false)
	{
		if (invokedMove)
		{
			if (isDirectional)
			{
				if (isTowardsEnd)
				{
					moveToEnd = true;
					directedTowardsEnd = true;
				}
				else
				{
					moveToEnd = false;
					directedTowardsStart = true;
				}
			}
			else
			{
				directedTowardsEnd = false;
				directedTowardsStart = false;
			}
			isMoving = true;
		}
		else
		{
			isMoving = false;
		}
	}

	private IEnumerator DelayAtTarget()
	{
		yield return new WaitForSeconds(1.5f);
		isDelayed = false;
	}
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventDisplay : MonoBehaviour
{
	public static EventDisplay instance;
	private Queue<EventInfo> events;

	private RectTransform eventTransform;

	public GameObject eventPrefab;
	public GameObject eventList;

	private float lifeTime;
	private float fadeTime;

	private float xMinLeft;
	private float xMaxLeft;
	private float xMinRight;
	private float xMaxRight;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
		events = new Queue<EventInfo>();
	}

	void Start()
	{
		eventTransform = GetComponent<RectTransform>();

		lifeTime = 2.5f;
		fadeTime = 0.5f;

		xMinLeft = 0.007f;
		xMaxLeft = 0.243f;
		xMinRight = 0.757f;
		xMaxRight = 0.993f;
	}

	private void FixedUpdate()
	{
		if (events.Count > 0)
		{
			if (Time.time - events.Peek().timestamp > lifeTime)
			{
				if (!events.Peek().fadeStarted)
				{
					events.Peek().reference.GetComponent<Animator>().SetTrigger("FadeOut");
					events.Peek().fadeStarted = true;
				}
				if (Time.time - events.Peek().timestamp > lifeTime + fadeTime)
				{
					Destroy(events.Peek().reference);
					events.Dequeue();
					if (events.Count > 0)
					{
						events.Peek().timestamp = Time.time - 1.0f;
					}
				}
			}
		}
	}

	public void AddEvent(string text)
	{
		int eventCount = events.Where(e => e.text == text).Select(e =>
		{
			e.count++;
			e.timestamp = Time.time;
			e.reference.GetComponent<EventPrefabReference>().text.text = e.text;
			e.reference.GetComponent<EventPrefabReference>().count.text = e.count.ToString();
			e.fadeStarted = false;
			return e;
		}).Count();

		if (eventCount == 0)
		{
			InstantiateEvent(text);
		}
	}

	private void InstantiateEvent(string text)
	{
		var eventToAdd = new EventInfo();
		eventToAdd.text = text;
		eventToAdd.count = 1;
		eventToAdd.timestamp = Time.time;
		events.Enqueue(eventToAdd);

		//instantiate a prefab for the event
		GameObject newEvent = Instantiate(eventPrefab) as GameObject;
		EventPrefabReference controller = newEvent.GetComponent<EventPrefabReference>();

		//set the text for the event onscreen and provide a reference to it in the queue
		controller.text.text = text;
		controller.count.text = "1";
		eventToAdd.reference = controller.gameObject;

		//put the event
		newEvent.transform.SetParent(eventList.transform);

		//for some reason Unity does not use full scale for the instantiated object by default
		newEvent.transform.localScale = Vector3.one;
		newEvent.transform.localRotation = Quaternion.Euler(Vector3.zero);
	}

	public void ShiftContainer(bool isLeft)
	{
		if (isLeft)
		{
			eventTransform.anchorMin = new Vector2(xMinLeft, eventTransform.anchorMin.y);
			eventTransform.anchorMax = new Vector2(xMaxLeft, eventTransform.anchorMax.y);
		}
		else
		{
			eventTransform.anchorMin = new Vector2(xMinRight, eventTransform.anchorMin.y);
			eventTransform.anchorMax = new Vector2(xMaxRight, eventTransform.anchorMax.y);
		}
	}
}

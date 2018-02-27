using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Interactable
{
    private float collectTime;
    public int id;

    //used for initialization
    private new void Start()
    {
        typeOfInteractable = Types.Pickup;
        base.Start();
        collectTime = 1.0f;
    }

    public void StartCollectRoutine(GameObject player)
    {
        StartCoroutine(Collect(player));
    }

    private IEnumerator Collect(GameObject player)
    {
        float collectStart = Time.time;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        while (Time.time - collectStart < collectTime)
        {
            transform.localScale = transform.localScale * 0.95f;
            transform.position = Vector3.Lerp(transform.position, player.transform.position, (Time.time - collectStart) / collectTime);
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }

    public void GenerateID()
    {
        id = Mathf.RoundToInt(Random.Range(System.Int32.MinValue, System.Int32.MaxValue));
    }

}

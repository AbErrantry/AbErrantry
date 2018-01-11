using UnityEngine;
using System.Collections;

public class BackDoor : Interactable
{
    public GameObject doorPair;
    private Animator anim;
    private float openTime;
    private new void Start()
    {
        typeOfInteractable = Types.BackDoor;
        base.Start();
        anim = GetComponent<Animator>();
        openTime = 0.25f;
    }

    public IEnumerator EnterDoor(GameObject character)
    {
        float startTime;
        startTime = Time.time;
        anim.SetBool("isOpen", true);
        while (Time.time - startTime < openTime)
        {
            yield return null;
        }
        character.transform.position = doorPair.transform.position;
        anim.SetBool("isOpen", false);
    }
}

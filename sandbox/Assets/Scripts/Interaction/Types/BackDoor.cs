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

    public IEnumerator EnterDoor(GameObject character, bool isFirst)
    {
        float startTime;
        startTime = Time.time;
        anim.SetBool("isOpen", true);

        yield return new WaitForSeconds(openTime);

        anim.SetBool("isOpen", false);
        if(isFirst)
        {
            character.GetComponent<Character2D.Player>().ToggleCamera(false);
            character.transform.position = doorPair.transform.position;
            character.GetComponent<Character2D.Player>().ToggleCamera(true);
            yield return StartCoroutine(doorPair.GetComponent<BackDoor>().EnterDoor(character, false));
        }
    }
}

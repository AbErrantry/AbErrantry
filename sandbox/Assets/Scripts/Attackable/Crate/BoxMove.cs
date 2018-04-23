using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMove : Trigger<Character2D.Player>
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    public bool canMove;

    private FMOD.Studio.EventInstance crateBump;

    private void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        boxCollider = transform.parent.GetComponent<BoxCollider2D>();
        canMove = true;

        crateBump = FMODUnity.RuntimeManager.CreateInstance("event:/Crate/impact");
        crateBump.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));
        crateBump.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(GetComponent<Transform>(), GetComponent<Rigidbody>()));
    }

    //fires upon an object entering/exiting the trigger box
    protected override void TriggerAction(bool isInTrigger)
    {
        if (isInTrigger && canMove)
        {
            rb.mass = 0.1f;
            boxCollider.enabled = false;
            crateBump.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(GetComponent<Transform>(), GetComponent<Rigidbody>()));
            crateBump.start();
        }
        else
        {
            boxCollider.enabled = true;
            rb.mass = 25.0f;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }

    public void SetCanMove(bool inMove)
    {
        if (inMove)
        {
            canMove = true;
            TriggerAction(currentObjects.Count > 0);
        }
        else
        {
            canMove = false;
            boxCollider.enabled = true;
            rb.mass = 25.0f;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }
}

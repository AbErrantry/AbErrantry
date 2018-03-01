using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMove : Trigger<Character2D.Player>
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    public bool canMove;
    public bool playerUnder;

    private void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        boxCollider = transform.parent.GetComponent<BoxCollider2D>();
        canMove = true;
        playerUnder = false;
    }

    //fires upon an object entering/exiting the trigger box
    protected override void TriggerAction(bool isInTrigger)
    {
        if (isInTrigger && (canMove || playerUnder))
        {
            rb.mass = 0.1f;
            boxCollider.enabled = false;
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
            if (!playerUnder)
            {
                boxCollider.enabled = true;
                rb.mass = 25.0f;
                rb.velocity = new Vector2(0.0f, rb.velocity.y);
            }
        }
    }

    public void SetPlayerUnder(bool inPlayer)
    {
        if (inPlayer)
        {
            playerUnder = true;
            rb.mass = 0.1f;
            boxCollider.enabled = false;
        }
        else
        {
            playerUnder = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour {

    Rigidbody2D _rb2d;
    BoxCollider2D _collider;

    void Awake()
    {
        _collider = gameObject.GetComponent<BoxCollider2D>();
        _rb2d = gameObject.AddComponent<Rigidbody2D>();
        _rb2d.isKinematic = true;
    }

    void SetNewCameraBounds()
    {
        CameraFollow cam = Camera.main.gameObject.GetComponent<CameraFollow>();
        //cam.SetNewBounds(_collider.bounds); changed follow script
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            SetNewCameraBounds();
        }
    }
}

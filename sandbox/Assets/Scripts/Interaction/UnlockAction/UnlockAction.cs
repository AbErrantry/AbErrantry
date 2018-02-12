using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnlockAction : MonoBehaviour
{
    public enum Types
    {
        ButtonsDown,
        DestroyGameObjects,
        HaveKey,
        CheckPoint,
        None
    }

    protected Openable openable;

    protected void Start()
    {
        openable = GetComponent<Openable>();
    }

    protected void UnlockOpenable()
    {
        openable.Unlock();
    }

    protected abstract void CheckUnlock();
}

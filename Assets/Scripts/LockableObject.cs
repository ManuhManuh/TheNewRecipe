using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockableObject : MonoBehaviour
{
    [SerializeField] protected GameObject disabledPart;
    public bool Locked
    {
        get
        {
            return locked;
        }

    }

    protected bool locked;

    protected virtual void Start()
    {
        locked = true;
    }

    public virtual void OnUnlocked()
    {
        disabledPart.SetActive(true);

    }


} 



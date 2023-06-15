using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableObject : MonoBehaviour
{
    public bool Used
    {
        get
        {
            return used;
        }

    }

    protected bool used;

    protected virtual void Start()
    {
        used = true;
    }

    public virtual void OnUsed()
    {
    }
}

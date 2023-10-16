using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForInteraction : MonoBehaviour
{
    public void LogInteraction(Collider thingy)
    {
        Debug.Log($"Interaction started with {thingy.gameObject.name}");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourRing : MonoBehaviour
{
    [SerializeField] Keg parentKeg;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Hit by {collision.gameObject.name}");
        parentKeg.RingHitDetected(collision.gameObject);
        
    }

}

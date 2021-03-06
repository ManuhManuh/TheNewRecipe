using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public GameObject brokenVersion;
    public string breakingSoundName;

    private GameObject currentBrokenVase;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Brick"))
        {
            // Instantiate the broken version
            currentBrokenVase = Instantiate(brokenVersion, transform.position, transform.rotation);

            // Play the breaking sound
            SoundManager.PlaySound(currentBrokenVase, breakingSoundName);

            // Destroy the unbroken version
            Destroy(gameObject);
        }
    }

}

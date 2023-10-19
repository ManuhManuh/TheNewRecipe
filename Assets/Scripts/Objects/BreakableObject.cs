using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public GameObject brokenVersion;
    public string breakingSoundName;

    [SerializeField] private GameObject hiddenObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Smasher"))
        {
            // Activate the broken version
            brokenVersion.SetActive(true);

            // Play the breaking sound
            SoundManager.PlaySound(brokenVersion, breakingSoundName);

            // if there is a hidden object, enable it
            if (hiddenObject != null)
            {
                hiddenObject.SetActive(true);
            }

            // Destroy the unbroken version
            Destroy(gameObject);

        }
    }

}

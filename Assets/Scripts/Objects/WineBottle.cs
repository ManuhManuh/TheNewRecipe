using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WineBottle : MonoBehaviour
{
    [SerializeField] InputActionReference showGhostObjects;

    [SerializeField] private List<AudioClip> glassClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> woodClips = new List<AudioClip>();
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float soundStartDelay;
    [SerializeField] private float soundSpacing;
    [SerializeField] private Material ghostMaterial;

    private bool soundAllowed = false;
    private bool inGhostMode = false;
    private MeshRenderer meshRenderer;
    private Material originalMaterial;
    
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalMaterial = meshRenderer.material;
        StartCoroutine(AllowSound(soundStartDelay));
    }

    private IEnumerator AllowSound(float delay)
    {
        yield return new WaitForSeconds(delay);
        soundAllowed = true;
    }

    private void Update()
    {
        float secondaryButtonValue = showGhostObjects.action.ReadValue<float>();
        if(secondaryButtonValue > 0.1 && !inGhostMode)
        {
            inGhostMode = true;
            ShowGhostBottle();
        }
        if(secondaryButtonValue < 0.3 && inGhostMode)
        {
            inGhostMode = false;
            HideGhostBottle();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"Collision with {collision.gameObject.name}");
        if (soundAllowed)
        {
            soundAllowed = false;
            if (collision.gameObject.CompareTag("WineBottle"))
            {
                // play a random glass clink sound
                audioSource.PlayOneShot(glassClips[Random.Range(0, glassClips.Count - 1)]);
            }
            else
            {
                // play a random wood clink sound
                audioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Count - 1)]);
            }
            StartCoroutine(AllowSound(soundSpacing));
        }
        
    }

    public void ShowGhostBottle()
    {
        meshRenderer.material = ghostMaterial;
    }

    public void HideGhostBottle()
    {
        meshRenderer.material = originalMaterial;
    }

}

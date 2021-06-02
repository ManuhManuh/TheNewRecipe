using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPointer : MonoBehaviour
{
    public int lineRange;
    public LineRenderer lineRend;
    public Vector3[] points;
    public LayerMask layerMask;
    public Button button;
    public Color32 hitColour;
    public Color32 notHitColour;
    private void Start()
    {

        lineRend = GetComponent<LineRenderer>();

        // Initialize the line renderer
        points = new Vector3[2];

        // Set the start point to the position of the hand/controller
        points[0] = Vector3.zero;

        // Set the end to 20m away from the hand
        points[1] = transform.position + new Vector3(0, 0, lineRange);

        // Set the positions array on the line renderer to the new values above
        lineRend.SetPositions(points);
        lineRend.enabled = true;

    }

    public bool AlignLineRenderer(LineRenderer lineRend)
    {
        Ray ray;
        ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        bool hitButton;

        if (Physics.Raycast(ray, out hit, layerMask))
        {
            // Update the end point of the line
            points[1] = transform.forward + new Vector3(0, 0, hit.distance);

            // Update the colour of the line
            lineRend.startColor = hitColour;
            lineRend.endColor = hitColour;

            // Get the button that was hit
            button = hit.collider.gameObject.GetComponent<Button>();

            // Update the hit flag
            hitButton = true;

            // Debug.Log("Hit");
        }
        else
        {
            // Update the end point of the line back to the max range
            points[1] = transform.forward + new Vector3(0, 0, lineRange);

            // Update the colour of the line
            lineRend.startColor = notHitColour;
            lineRend.endColor = notHitColour;

            // Update the hit flag
            hitButton = false;

            // Debug.Log("Not hit");
        }

        lineRend.SetPositions(points);
        lineRend.material.color = lineRend.startColor;
        return hitButton;
    }

    private void Update()
    {
        if (AlignLineRenderer(lineRend) && Input.GetAxis("Submit") > 0)
        {
            button.onClick.Invoke();
        }
    }

    public void SceneChangeOnClick()
    {
        
        if (button != null)
        {
            if (button.name == "PlayButton")
            {
                Debug.Log("Play: Load Intro scene");
            }
            else if(button.name == "ExitButton")
            {
                Debug.Log("Exit: exit to Quest Home");
            }
            else if(button.name == "CreditsButton")
            {
                Debug.Log("Credits: Load Credits scene");
            }
        }
    }
}

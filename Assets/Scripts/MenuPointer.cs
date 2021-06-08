using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuPointer : MonoBehaviour
{
    public int lineRange;
    public LineRenderer laserPointer;
    public Vector3[] points;
    public Button button;
    public Color32 hitColour;
    public Color32 notHitColour;
    public Button resumeButton;
    public TMP_Text playWarning;
    public string rightTriggerInput;

    private void Start()
    {

        laserPointer = GetComponent<LineRenderer>();

        // Initialize the line renderer
        points = new Vector3[2];

        // Set the start point to the position of the hand/controller
        points[0] = Vector3.zero;

        // Set the end to 20m away from the hand
        points[1] = transform.position + new Vector3(0, 0, lineRange);

        // Set the positions array on the line renderer to the new values above
        laserPointer.SetPositions(points);
        laserPointer.enabled = true;

        // Determine if the RESUME button and play warning should be enabled
        if (!SceneControl.newGame)
        {
            playWarning.enabled = true;
            resumeButton.enabled = true;
        }
    }

    public bool LaserPointerHitButton()
    {
        Ray ray;
        ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        bool hitButton;

        if (Physics.Raycast(ray, out hit))
        {
            // Update the end point of the line to the hit object
            //points[1] = transform.forward + new Vector3(0, 0, hit.distance);
            SetBeamEndPoint(hit.point);

            // Update the colour of the line
            laserPointer.startColor = hitColour;
            laserPointer.endColor = hitColour;

            // Get the button that was hit
            button = hit.collider.gameObject.GetComponent<Button>();

            // Update the hit flag
            hitButton = true;

            // Debug.Log("Hit");
        }
        else
        {
            // Update the end point of the line back to the max line range
            //points[1] = transform.forward + new Vector3(0, 0, lineRange);
            SetBeamEndPoint(transform.position + transform.forward * lineRange);

            // Update the colour of the line
            laserPointer.startColor = notHitColour;
            laserPointer.endColor = notHitColour;

            // Update the hit flag
            hitButton = false;

            // Debug.Log("Not hit");
        }

        laserPointer.SetPositions(points);
        laserPointer.material.color = laserPointer.startColor;
        return hitButton;
    }

    private void Update()
    {
        // if (AlignLineRenderer(laserPointer) && Input.GetAxis("Submit") > 0)
        if (LaserPointerHitButton() && Input.GetButtonDown(rightTriggerInput))

        {
            button.onClick.Invoke();
        }
    }

    private void SetBeamEndPoint(Vector3 endPoint)
    {
        // Set the start and end positions of the beam
        // beam.SetPosition(0, transform.position);    // set start position: 0 is index of start point
        // beam.SetPosition(1, endPoint);              // set end position: 1 is index of start point

        List<Vector3> points = new List<Vector3>();
        points.Add(transform.position);
        points.Add(endPoint);

        DrawBeam(points);
    }

    private void DrawBeam(List<Vector3> points)
    {
        // for loop to go though points in order
        laserPointer.positionCount = points.Count;


        for (int i = 0; i < points.Count; i++)
        {
            // Debug.Log("Point " + i + ": " + points[i].x + "," + points[i].y + "," + points[i].z);
            laserPointer.SetPosition(i, points[i]);
        }

    }

    public void MenuChoiceOnClick()
    {
        // If a button was recorded as a raycast hit
        if (button != null)
        {
            switch (button.name)
            {
                case "InstructionsButton":
                    {
                        SceneControl.OnMenuSelection(SceneControl.SceneAction.Instructions);
                        return;
                    }

                case "PlayButton":
                    {
                        SceneControl.OnMenuSelection(SceneControl.SceneAction.PlayIntro);
                        return;
                    }

                case "ResumeButton":
                    {
                        SceneControl.OnMenuSelection(SceneControl.SceneAction.Resume);
                        return;
                    }

                case "CreditsButton":
                    {
                        SceneControl.OnMenuSelection(SceneControl.SceneAction.Credits);
                        return;
                    }

                case "ExitButton":
                    {
                        SceneControl.OnMenuSelection(SceneControl.SceneAction.Exit);
                        return;
                    }
                case "MainMenuButton":
                    {
                        SceneControl.OnMenuSelection(SceneControl.SceneAction.None);
                        return;
                    }
            }
        }
    }
}

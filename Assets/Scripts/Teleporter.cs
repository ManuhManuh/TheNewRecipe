using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public string thumbstickBeamInputName;
    public float thumbstickForwardThreshold;
    public Color validColour;
    public Color invalidColour;
    public LineRenderer beam;
    public float range;
    public GameObject teleportIndicator;
    public Transform player;

    private bool hasValidTeleportTarget;
    private int validTargetLayerMask;
    private int teleportTargetLayer;

    void Start()
    {
        SetBeamVisible(false);
        teleportTargetLayer = LayerMask.NameToLayer("ValidTeleportTarget");
        validTargetLayerMask = 1 << teleportTargetLayer;
    }

    void Update()
    {
        // If the thumbstick is pressed forward
        if (Input.GetAxis(thumbstickBeamInputName) < thumbstickForwardThreshold)
        {
            // Show the teleport beam
            SetBeamVisible(true);

            // Extend beam to maximum range (tranform.position is hand in world)
            SetBeamEndPoint(transform.position + transform.forward * range);

            // Search for a valid teleport target
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, range, validTargetLayerMask);
            {
                // See if we hit a valid teleport target
                if (hits.Length > 0) 
                { 
                    // Grab the first thing hit (should only be one anyway)
                    RaycastHit hit = hits[0];

                    // Check if it is valid for the play mode selected
                    if (hit.transform.CompareTag("SittingTarget") && !GameManager.sitting)
                    {
                        // Set the beam to be invalid
                        SetTeleportValid(false);
                    }
                    else
                    {
                        // Update beam end point to the point in space it hit (so it doesn't pass though any objects)
                        SetBeamEndPoint(hit.point);

                        // Set the beam to be valid (which will change colour, show spot, etc.) 
                        SetTeleportValid(true);

                        // Set the position of the teleport indicator (just off the floor to avoid z-fighting)
                        teleportIndicator.transform.position = hit.point + Vector3.up * 0.001f;
                    }
                }
                else
                {
                    // Set the beam to be invalid
                    SetTeleportValid(false);
                }
            }
        }

        else     // The thumbstick has been released or is no longer being pushed forward
        {
            // Hide the teleport beam
            SetBeamVisible(false);

            // If we have a valid teleport target
            if (hasValidTeleportTarget)
            {
                // Teleport the player there
                player.position = teleportIndicator.transform.position;

                // Remove the target indicator
                teleportIndicator.SetActive(false);
            }

        }
    }

    private void SetBeamVisible(bool visible)
    {
        beam.enabled = visible;
    }

    private void SetTeleportValid(bool valid)
    {
        // Set the appropriate colour of the beam
        beam.material.color = valid ? validColour : invalidColour;

        // Show or hide the teleport indicator
        teleportIndicator.SetActive(valid);

        // Remember if we have a valid target or not
        hasValidTeleportTarget = valid;
    }

    private void SetBeamEndPoint(Vector3 endPoint)
    {
        // Set the start and end positions of the beam
        // beam.SetPosition(0, transform.position);    // set start position: 0 is index of start point
        // beam.SetPosition(1, endPoint);              // set end position: 1 is index of start point

        List<Vector3> points = new List<Vector3>();
        points.Add(transform.position);
        points.Add(endPoint);
        points = MakeSmoothCurve(points, 3.0f);

        DrawBeam(points);
    }

    private void DrawBeam(List<Vector3> points)
    {
        // for loop to go though points in order
        beam.positionCount = points.Count;

        
        for (int i = 0; i < points.Count; i++)
        {
            // Debug.Log("Point " + i + ": " + points[i].x + "," + points[i].y + "," + points[i].z);
            beam.SetPosition(i, points[i]);
        }

    }

    public static List<Vector3> MakeSmoothCurve(List<Vector3> arrayToCurve, float smoothness)
    {
        List<Vector3> points;
        List<Vector3> curvedPoints;
        int pointsLength = 0;
        int curvedLength = 0;

        if (smoothness < 1.0f) smoothness = 1.0f;

        pointsLength = arrayToCurve.Count;

        curvedLength = (pointsLength * Mathf.RoundToInt(smoothness)) - 1;
        curvedPoints = new List<Vector3>(curvedLength);

        float t = 0.0f;
        for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++)
        {
            t = Mathf.InverseLerp(0, curvedLength, pointInTimeOnCurve);

            points = new List<Vector3>(arrayToCurve);

            for (int j = pointsLength - 1; j > 0; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    points[i] = (1 - t) * points[i] + t * points[i + 1];
                }
            }

            curvedPoints.Add(points[0]);
        }

        return (curvedPoints);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitMovement : MonoBehaviour
{
    /// <summary>
    /// This function will limit the movement of an object in a single direction relative to its starting position
    /// For example, it can stop a drawer from sinking into the cabinet, or a door from sliding too far
    /// 
    /// </summary>

    [SerializeField] private bool limitX;
    [SerializeField] private bool limitY;
    [SerializeField] private bool limitZ;
    [SerializeField] private bool keepGreaterThan;

    private float startX;
    private float startY;
    private float startZ;
    private Vector3 homePosition;

    private void Start()
    {
        startX = transform.position.x;
        startY = transform.position.y;
        startZ = transform.position.z;
    }
    // Update is called once per frame
    void Update()
    {
        if (limitX && transform.position.x != startX)
        {
            if (keepGreaterThan)
            {
                if(transform.position.x < startX)
                {
                    transform.position = new Vector3(startX, transform.position.y, transform.position.z);
                }
            }
            else
            {
                if (transform.position.x > startX)
                {
                    transform.position = new Vector3(startX, transform.position.y, transform.position.z);
                }
            }
        }
        if (limitY && transform.position.y != startY)
        {
            if (keepGreaterThan)
            {
                if (transform.position.y < startY)
                {
                    transform.position = new Vector3(transform.position.x, startY, transform.position.z);
                }
            }
            else
            {
                if (transform.position.y > startZ)
                {
                    transform.position = new Vector3(transform.position.x, startY, transform.position.z);
                }
            }
        }
        if (limitZ && transform.position.z != startY)
        {
            if (keepGreaterThan)
            {
                if (transform.position.z < startZ)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, startZ);
                }
            }
            else
            {
                if (transform.position.z > startZ)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, startZ);
                }
            }
        }
    }
}

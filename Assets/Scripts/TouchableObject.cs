using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchableObject : MonoBehaviour
{
    public Color touchedColour;
    public bool changeColour;

    private Color initialColour;
    protected private Rigidbody rigidBody;

    // Start is called before the first frame update
    protected void Start()
    {
        // Store the initial colour
        if (changeColour)
        {
            initialColour = GetComponent<Renderer>().material.color;
        }
        rigidBody = GetComponent<Rigidbody>();
    }

    public virtual void OnTouched()
    {
        if (changeColour)
        {
            // Change colour of the object to the touched colour
            GetComponent<Renderer>().material.color = touchedColour;
        }
    }

    public virtual void OnUntouched()
    {
        if (changeColour)
        {
            // Change the colour back to the initial colour
            GetComponent<Renderer>().material.color = initialColour;
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    public string nameOfButton;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(nameOfButton))
        {
            Debug.Log("This is it!!");
        }
    }
}

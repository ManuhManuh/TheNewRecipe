using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputReader : MonoBehaviour
{
    public List<InputDevice> inputDevices = new List<InputDevice>();

    // Start is called before the first frame update
    void Start()
    {
        SearchForInputDevices();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputDevices.Count < 3)
        {
            SearchForInputDevices();
        }
    }

    private void SearchForInputDevices()
    {
        InputDevices.GetDevices(inputDevices);

    }
}

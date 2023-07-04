using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]

public class AnimateHandController : MonoBehaviour
{
    public InputActionReference gripInputAction;
   // public InputActionReference triggerInputAction;
    private Animator handAnimator;
    public float gripValue;

    //private float triggerValue;

    // Start is called before the first frame update
    void Awake()
    {
       handAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimateGrip();
        //AnimateTrigger();
    }

    private void AnimateGrip()
    {
        gripValue = gripInputAction.action.ReadValue<float>();
        if (gripValue < 0.1)
        {
            gripValue = 0;
        }
        if(handAnimator != null)
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator = GetComponent<Animator>();
        }
        
    }

    //private void AnimateTrigger()
    //{
    //    triggerValue = triggerInputAction.action.ReadValue<float>();
    //    handAnimator.SetFloat("Trigger", gripValue);
    //}
}

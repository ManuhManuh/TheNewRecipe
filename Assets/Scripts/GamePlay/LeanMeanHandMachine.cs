using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeanMeanHandMachine : MonoBehaviour
{
    [SerializeField] InputActionProperty gripAction;
    [SerializeField] InputActionProperty triggerAction;

    private Animator handAnimator;

    private void Awake()
    {
        //controllerActionGrip.action.performed += GripPress;
        //controllerActionTrigger.action.performed += TriggerPress;
    }
    private void Start()
    {
        handAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        float triggerValue = triggerAction.action.ReadValue<float>();
        float gripValue = gripAction.action.ReadValue<float>();

        handAnimator.SetFloat("Grip", gripValue);
        handAnimator.SetFloat("Trigger", triggerValue);

    }

    //private void GripPress(InputAction.CallbackContext obj) => _handAnimator.SetFloat("Grip", obj.ReadValue<float>());
    //private void TriggerPress(InputAction.CallbackContext obj) => _handAnimator.SetFloat("Trigger", obj.ReadValue<float>());
}

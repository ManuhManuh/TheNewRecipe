using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeanMeanHandMachine : MonoBehaviour
{
    [SerializeField] InputActionProperty gripAction;
    [SerializeField] InputActionProperty triggerAction;
    [SerializeField] InputActionReference returnToMainAction;

    private Animator handAnimator;
    // private SceneControl sceneControl;
    private Transform pausedPosition;

    private void Awake()
    {
        //controllerActionGrip.action.performed += GripPress;
        //controllerActionTrigger.action.performed += TriggerPress;

    }
    private void Start()
    {
        handAnimator = GetComponent<Animator>();
        // sceneControl = FindObjectOfType<SceneControl>();
        returnToMainAction.action.performed += MainMenuButtonPress;
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
    private void MainMenuButtonPress(InputAction.CallbackContext obj)
    {
        Debug.Log($"Current SceneAction: {SceneControl.instance.currentSceneAction}");
        if(SceneControl.instance.currentSceneAction != SceneControl.SceneAction.Menu)
        {
            SceneControl.OnMenuSelection(SceneControl.SceneAction.Menu);
        }
        
    }
    
}

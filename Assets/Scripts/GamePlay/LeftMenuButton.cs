using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LeftMenuButton : MonoBehaviour
{
    [SerializeField] InputActionReference returnToMainAction;

    private void Start()
    {
        returnToMainAction.action.performed += MainMenuButtonPress;

    }

    private void MainMenuButtonPress(InputAction.CallbackContext obj)
    {
        //Debug.Log($"MainMenu pressed by {this.gameObject.name}");

        if (!this.gameObject.activeInHierarchy) return;

        SceneControl.instance.ShowMainMenu();

    }

  
    
}

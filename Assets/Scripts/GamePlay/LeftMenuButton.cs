using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeftMenuButton : MonoBehaviour
{
    [SerializeField] InputActionReference returnToMainAction;

    private void Start()
    {
        returnToMainAction.action.performed += MainMenuButtonPress;
    }

    private void MainMenuButtonPress(InputAction.CallbackContext obj)
    {
        if (!this.gameObject.activeInHierarchy) return;

        if(SceneConductor.instance.ChapterIsOpen)
        {
            SceneConductor.instance.ShowMainMenuInChapter();
        }
        else
        {
            // pressed while in UI scene
            if(SceneConductor.instance.CurrentScene != (int)SceneConductor.SceneIndex.MainMenu)
            {
                SceneConductor.instance.ShowNonChapterScene(SceneConductor.SceneIndex.MainMenu);
            }
            
        }
        
        
    }
    
}

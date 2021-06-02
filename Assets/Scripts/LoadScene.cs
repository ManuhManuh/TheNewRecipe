using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string leftTriggerName;
    public string rightTriggerName;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(leftTriggerName))
        {
            if (SceneManager.GetActiveScene().name == "OptionsMenu")
            {
                SceneManager.LoadScene("MainMenu");
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu"));
            }
            else
            {
                SceneManager.LoadScene("OptionsMenu");
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("OptionsMenu"));
            }
            
        }
        if (Input.GetButtonDown(rightTriggerName))
        {
            SceneManager.LoadScene("Chapter01");
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Chapter01"));
        }

    }
}

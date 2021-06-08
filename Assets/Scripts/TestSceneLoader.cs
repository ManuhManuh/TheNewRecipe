using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneLoader : MonoBehaviour
{
    public string nextScene;

    public void LoadScene()
    {
        SceneManager.LoadScene(nextScene);
    }

}

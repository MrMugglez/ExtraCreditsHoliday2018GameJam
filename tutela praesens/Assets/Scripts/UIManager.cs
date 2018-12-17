using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void LoadLevel(int levelNum)
    {
        Debug.Log("Loading " + SceneManager.GetSceneByBuildIndex(levelNum).name + "...");
        SceneManager.LoadScene(levelNum);
    }

    public void Quit()
    {
        Debug.Log("Thank you, come again!");
        Application.Quit();
    }
}

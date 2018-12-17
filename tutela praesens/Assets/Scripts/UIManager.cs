using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Slider EnemyHealthBar;
    public Slider PlayerHealthBar;

    public Text Score;
    public Text Level;

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

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == GameManager.GAME) // Debugging/In Editor Purposes
        {
            PlayerHealthBar.maxValue = GameManager.instance.PlayerScript.MaxHealth;
            EnemyHealthBar.maxValue = GameManager.instance.EnemyScript.MaxHealth;
            PlayerHealthBar.minValue = 0;
            EnemyHealthBar.minValue = 0;
        }
    }

    // Only runs in GAME scene
    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == GameManager.GAME) // Debugging/In Editor Purposes
        {
            PlayerHealthBar.value = GameManager.instance.PlayerScript.CurrentHealth;
            EnemyHealthBar.value = GameManager.instance.EnemyScript.CurrentHealth;
            Score.text = string.Format("Score: {0}", GameManager.instance.Score.ToString());
            Level.text = string.Format("Level: {0}", GameManager.instance.Level.ToString());
        }
        else if (SceneManager.GetActiveScene().buildIndex == GameManager.GAMEOVER)
        {
            Level.text = string.Format("FINAL SCORE: {0}", GameManager.instance.Score.ToString());
        }
    }
}

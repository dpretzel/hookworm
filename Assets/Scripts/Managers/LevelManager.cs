using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // levels
    //private List<Level> levels;
    public static int currentLevelIndex = 0;
    public static LevelManager instance;

    private void Awake()
    {
        instance = this;
        //SceneManager.LoadScene(currentLevelIndex);
    }

    private void ToNextLevel()
    {
        //print("in the future this will actually get the next level :)");
        SceneManager.LoadScene(++currentLevelIndex);
        //restartLevel();
    }

    private void RestartLevel() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }

    private void Start()
    {
        // For the Title screen, start listening to OnPlay; otherwise, listen for the Scoreboard OK at the end of the level before advancing.
        TitleWindow t = FindObjectOfType<TitleWindow>();
        if (t)
            t.OnPlay += ToNextLevel;
        else
            FindObjectOfType<ScoreboardWindow>().OnWindowDestroy += ToNextLevel;

        GameplayManager.instance.onDeath += ListenForEarlyRestart;
        PlayerManager.instance.onDeathAnimationFinish += RestartLevel;
    }

    private void ListenForEarlyRestart()
    {
        InputManager.instance.onPrimaryPressed += RestartLevel;
    }
}

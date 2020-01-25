using System.Collections;
using System.Collections.Generic;
using _SGJ2020.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuActions : MonoBehaviour
{
    public void StartGame()
    {
        StateHolder.State.CurrentScreen = GameScreen.Game;
        SceneManager.LoadScene("RunnerLevel");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

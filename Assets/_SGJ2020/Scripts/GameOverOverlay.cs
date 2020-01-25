using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _SGJ2020.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverOverlay : MonoBehaviour
{
    private void Update()
    {
        Debug.Log(StateHolder.State.CurrentScreen);
        if (StateHolder.State.CurrentScreen == GameScreen.GameOver)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        //gameObject.SetActive(true);
        if (StateHolder.State.CurrentScreen == GameScreen.GameOver)
        {
            if (Input.anyKey)
            {
                StateHolder.State.CurrentScreen = GameScreen.Game;
                SceneManager.LoadScene("RunnerLevel");
            }
        }
    }
}

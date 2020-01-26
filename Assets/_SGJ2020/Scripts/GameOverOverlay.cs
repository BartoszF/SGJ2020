using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _SGJ2020.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverOverlay : MonoBehaviour
{
    private float _waitTime = 0;
    private void Update()
    {
        if (StateHolder.State.CurrentScreen == GameScreen.GameOver)
        {
            _waitTime += Time.deltaTime;
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else
        {
            _waitTime = 0;
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        //gameObject.SetActive(true);
        if (StateHolder.State.CurrentScreen == GameScreen.GameOver)
        {
            if (_waitTime > 1f && Input.anyKey)
            {
                StateHolder.State.CurrentScreen = GameScreen.Game;
                SceneManager.LoadScene("RunnerLevel");
            }
        }
    }
}

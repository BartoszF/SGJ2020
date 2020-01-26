using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{

    public static System.Random rnd = new System.Random();

    public PlayerTrigger playerTrigger;
    public AudioClip deathSound;

    protected GameObject _player;

    public void Start() {
        playerTrigger.registerListener(this);
    }

    public void PlayerLostSight() {
        _player = null;
    }
    public void PlayerInSight(GameObject playerObject) {
        _player = playerObject;
    }


    public void Kill()
    {
        var deathObj = new GameObject("Death Sound");
        var audioSource = deathObj.AddComponent(typeof(AudioSource)) as AudioSource;
        audioSource.PlayOneShot(deathSound);
        Destroy(deathObj,1f);
        Destroy(gameObject);
    }
}

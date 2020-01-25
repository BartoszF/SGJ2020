using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public static System.Random rnd = new System.Random();

    public PlayerTrigger playerTrigger;

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

    public void Hit() {
        Destroy(this.gameObject);
    }


}

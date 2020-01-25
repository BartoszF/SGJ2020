using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public PlayerTrigger playerTrigger;

    protected GameObject _player;

    public void Start() {
        playerTrigger.registerListener(this);
    }

    public void PlayerLostSight() {
        _player = null;
        Debug.Log("Lost sight");
    }
    public void PlayerInSight(GameObject playerObject) {
        _player = playerObject;
        Debug.Log("Player in sight");
    }

    public void Hit() {
        Destroy(this.gameObject);
    }


}

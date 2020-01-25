using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private Enemy _listener;
    public PlayerTrigger registerListener(Enemy listener) {
        _listener = listener;
        return this;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            _listener.PlayerInSight(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            _listener.PlayerLostSight();
        }
    }
}

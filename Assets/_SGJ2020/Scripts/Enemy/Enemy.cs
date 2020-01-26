using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Enemy : MonoBehaviour
{

    public static System.Random rnd = new System.Random();

    public PlayerTrigger playerTrigger;
    public AudioClip deathSound;

    protected GameObject _player;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;

    public void Start() {
        playerTrigger.registerListener(this);
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        _spriteRenderer.flipX = _rigidbody.velocity.x >= 0;
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

﻿using System;
using _SGJ2020.Scripts;
using _SGJ2020.Scripts.PPEffects;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    public bool godMode = false;

    public Transform rayDownLeftOrigin, rayDownRightOrigin;

    public Transform rayLeftOrigin, rayRightOrigin;

    public Transform sprite;
    public SpriteRenderer playerSprite;
    public Animator playerAnimator;
    public Transform playerSpriteWrapper;

    public PostProcessVolume ppv;

    public float jumpVelocity = 5f;
    public Vector2 wallJumpVelocity = new Vector2(5, 5);

    public float horizontalVelocity = 10f;

    public float maxSpeed = 5f;
    public float slowedMaxSpeed = 2f;
    public float maxHorizontalAirSpeed = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [FormerlySerializedAs("damagedBySpikesColddown")]
    public float damageColddown = 1f;

    public AudioClip JumpSound;
    public AudioClip gameOverSound;
    public AudioClip gameOverMusic;

    private bool _damaged = false;
    private float _currentDamageColddown = 0;

    public bool shootingDisabled = false;

    private bool _drunk = false;
    private float _drunkLerp = 0f;

    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;

    public int maxHp = 5;
    public float hpRegenTime = 10f;

    private int _currentHp;
    private float _currentHpRegenTime;
    private AudioSource _audioSource;
    public int CurrentHealth => _currentHp;

    public void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>();
        _currentHp = maxHp;
    }

    public void OnDrawGizmos()
    {
         // var raycastDirection = Physics2D.gravity.y < 0 ? -transform.up : transform.up;
         // Gizmos.DrawLine(rayDownLeftOrigin.position, rayDownLeftOrigin.position + raycastDirection * 0.2f);
    }

    public void FixedUpdate()
    {
        Vector2 result = _rigidbody2D.velocity;
        var raycastDirection = Physics2D.gravity.y < 0 ? -transform.up : transform.up;
        var floorLeft = Physics2D.Raycast(rayDownLeftOrigin.position, raycastDirection, 0.05f);
        var floorRight = Physics2D.Raycast(rayDownRightOrigin.position, raycastDirection, 0.05f);

        var isOnFloor = floorLeft.collider && floorLeft.collider.CompareTag("Ground") ||
                        floorRight.collider && floorRight.collider.CompareTag("Ground");

        var right = transform.right;
        var leftWall = (Physics2D.Raycast(rayLeftOrigin.position, -right, 0.1f));
        var rightWall = Physics2D.Raycast(rayRightOrigin.position, right, 0.1f);

        bool isOnLeftWall = leftWall.collider && leftWall.collider.CompareTag("Ground");
        bool isOnRightWall = rightWall.collider && rightWall.collider.CompareTag("Ground");

        if (isOnLeftWall || isOnRightWall)
        {
            playerAnimator.SetBool("rest", true);
        }


        Debug.Log(isOnFloor);
        if (isOnFloor && Input.GetButton("Jump"))
        {
            result += (Vector2)transform.up * (jumpVelocity * (Physics2D.gravity.y < 0 ? 1f : -1f));
            _audioSource.PlayOneShot(JumpSound);
            playerAnimator.SetBool("isJumping", true);
        }
        else if (!isOnFloor)
        {
            playerAnimator.SetBool("isJumping", true);
        }
        else if (isOnFloor)
        {
            playerAnimator.SetBool("isJumping", false);
            playerAnimator.SetBool("rest", true);


        }

        if (!isOnFloor && isOnLeftWall && Input.GetButton("Jump"))
        {
            Vector2 direction = new Vector2();
            direction += (Vector2)transform.right * wallJumpVelocity.x;
            direction += (Vector2)transform.up * wallJumpVelocity.y * (Physics2D.gravity.y < 0 ? 1f : -1f);
            result += direction;
            _audioSource.PlayOneShot(JumpSound);
            playerAnimator.SetBool("isJumping", true);
        }

        if (!isOnFloor && isOnRightWall && Input.GetButton("Jump"))
        {
            Vector2 direction = new Vector2();
            direction += -(Vector2)transform.right * wallJumpVelocity.x;
            direction += (Vector2)transform.up * wallJumpVelocity.y * (Physics2D.gravity.y < 0 ? 1f : -1f);
            result += direction;
            _audioSource.PlayOneShot(JumpSound);
            playerAnimator.SetBool("isJumping", true);
        }


        var drunkMultiplier = _drunk ? (Mathf.Sin(Time.timeSinceLevelLoad) > 0 ? 1f : -1f) : 1f;
        if (Math.Abs(Input.GetAxis("Horizontal")) > 0.1f)
        {
            result += (Vector2)transform.right *
                      (Input.GetAxis("Horizontal") * horizontalVelocity * Time.fixedDeltaTime * drunkMultiplier);
            playerAnimator.SetBool("isJumping", false);
            playerAnimator.SetBool("rest", false);
        }
        else if (isOnFloor)
        {
            result.x *= 0.2f * Time.fixedDeltaTime;
            playerAnimator.SetBool("rest", true);

        }


        if (_rigidbody2D.velocity.y < 0)
        {
            result += (Vector2)transform.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime);
        }
        else if (_rigidbody2D.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            result += (Vector2)transform.up * (Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime);
        }

        if (isOnFloor)
        {
            var currentMaxSpeed = _damaged ? slowedMaxSpeed : maxSpeed;
            if (Math.Abs(result.x) > currentMaxSpeed)
            {
                result.x = Math.Sign(result.x) * currentMaxSpeed;
            }
        }
        else
        {
            if (Math.Abs(result.x) > maxHorizontalAirSpeed)
            {
                result.x = Math.Sign(result.x) * maxHorizontalAirSpeed;
            }
        }

        var currentXScale = playerSpriteWrapper.localScale.x;

        if (result.x > 0.01f)
        {
            playerSpriteWrapper.localScale = new Vector3(currentXScale > 0 ? currentXScale : currentXScale * -1, playerSpriteWrapper.localScale.y,
                1);
        }
        else if (result.x < -0.01f)
        {
            playerSpriteWrapper.localScale = new Vector3(currentXScale < 0 ? currentXScale : currentXScale * -1, playerSpriteWrapper.localScale.y,
                1);
        }

        var scaleY = Physics2D.gravity.y < 0f ? 1f : -1f;
        transform.localScale = new Vector3(transform.localScale.x, scaleY, 1);

        _rigidbody2D.velocity = result;
        _currentDamageColddown -= Time.deltaTime;
        if (_currentDamageColddown < -0)
        {
            _damaged = false;
        }

        playerSprite.color = _damaged ? Color.red : Color.white;

        _drunkLerp = _drunk
            ? Mathf.Clamp(_drunkLerp + Time.deltaTime, 0f, 1f)
            : Mathf.Clamp(_drunkLerp - Time.deltaTime, 0f, 1f);
        var drunkPP = ppv.profile.settings.FindLast(s => s.name == "DrunkEffect(Clone)") as DrunkEffect;
        drunkPP.waving.value = _drunkLerp * 0.01f;
        drunkPP.duplicating.value = _drunkLerp * 0.01f;

        if (_currentHp == 0)
        {
            StateHolder.State.CurrentScreen = GameScreen.GameOver;
            AudioSource cameraSource = Camera.main.gameObject.GetComponent<AudioSource>();
            cameraSource.clip = gameOverMusic;
            cameraSource.Play();
            var gameOverSoundObject = new GameObject("GameOverSound");
            var gameOverSource = gameOverSoundObject.AddComponent(typeof(AudioSource)) as AudioSource;
            gameOverSource.PlayOneShot(gameOverSound);
            gameObject.SetActive(false);
        }
        if (!_damaged && _currentHp < maxHp)
        {
            _currentHpRegenTime += Time.deltaTime;
            if (_currentHpRegenTime > hpRegenTime)
            {
                _currentHp++;
                _currentHpRegenTime = 0;
            }
        }
        else
        {
            _currentHpRegenTime = 0;
        }
    }

    public void DamageBySpikes()
    {
        if (_damaged || godMode)
        {
            return;
        }
        _currentDamageColddown = damageColddown;
        _damaged = true;
        _currentHp--;
    }

    public void SetDrunk(bool drunk)
    {
        _drunk = drunk;
    }

    public void SetNoShooting(bool noShooting)
    {
        shootingDisabled = noShooting;
    }

    public void DamageByPlatform(Vector3 transformPosition)
    {
        if (_damaged || godMode)
        {
            return;
        }
        var forceVector = (-transformPosition + transform.position).normalized * 10f;
        _rigidbody2D.AddForce(forceVector, ForceMode2D.Impulse);
        _damaged = true;
        _currentDamageColddown = damageColddown;
        _currentHp--;
    }

    public void DamageByBullet()
    {
        if (_damaged || godMode)
        {
            return;
        }
        _damaged = true;
        _currentDamageColddown = damageColddown;
        _currentHp--;
    }
}
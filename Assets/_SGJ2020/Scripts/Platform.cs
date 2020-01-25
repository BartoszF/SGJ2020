using System;
using System.Linq;
using _SGJ2020.Scripts.GameplayEffects;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Platform : MonoBehaviour
{
    private GameplayEffectManager _manager;
    private Collider2D _collider2D;
    private Collider2D _playerCollider;
    private PlayerController _playerController;
    private SpriteRenderer _spriteRenderer;

    public float heatingTime = 1f;
    public float damageInterval = 0.5f;
    
    private float _heatingProgress; 

    public void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
        _manager = GameObject.FindGameObjectsWithTag("Manager").First().GetComponent<GameplayEffectManager>();
        var playerGO = GameObject.FindGameObjectsWithTag("Player").First();
        _playerCollider = playerGO.GetComponent<Collider2D>();
        _playerController = playerGO.GetComponent<PlayerController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        if (_manager.heating)
        {
            if (_collider2D.IsTouching(_playerCollider))
            {
                _heatingProgress += Time.deltaTime;
                if (_heatingProgress > heatingTime)
                {
                    _playerController.DamageByPlatform(_collider2D.transform.position);
                    _heatingProgress -= damageInterval;
                }
            }
            else
            {
                _heatingProgress = Mathf.Clamp(_heatingProgress -= Time.deltaTime, 0, heatingTime);
            }
        }
        else
        {
            _heatingProgress = 0f;
        }

        _spriteRenderer.color = Color.Lerp(Color.white, Color.yellow, _heatingProgress / heatingTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker), typeof(BulletShooter))]
public class FlyingEnemy : Enemy
{

    public Transform Player;
    public float Speed;
    public float shootCooldown = 3f;
    public float shootForce = 5f;

    private BulletShooter _bulletShooter;
    private float _shootTimer = 0f;

    private Seeker _seeker;
    private Rigidbody2D _rigidbody;

    private Path _path;
    private int _curPoint;
    private bool _reachedEnd = false;

    void Start()
    {
        base.Start();

        _seeker = GetComponent<Seeker>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _bulletShooter = GetComponent<BulletShooter>();

        InvokeRepeating("UpdatePath", 0f, 2f);
    }

    void UpdatePath()
    {
        _seeker.StartPath(transform.position, Player.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _curPoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_shootTimer > 0)
        {
            _shootTimer -= Time.deltaTime;
        }
        
        if (_player)
        {
            Vector2 playerPosition = _player.transform.position;
            Vector2 diff = playerPosition - (Vector2)transform.position;

            if (_shootTimer <= 0f)
            {
                var target = Physics2D.Raycast((Vector2)transform.position + diff.normalized, diff.normalized, 15);

                if (!target.collider || target.collider.tag == "Player")
                {
                    _bulletShooter.Shoot((Vector2)transform.position + diff.normalized, diff * shootForce);
                    _shootTimer = shootCooldown;
                }
            }
        }
        if (_path != null)
        {
            if (_curPoint >= _path.vectorPath.Count)
            {
                _reachedEnd = true;
            }
            else
            {
                _reachedEnd = false;
            }

            Vector2 dir = ((Vector2)_path.vectorPath[_curPoint] - (Vector2)transform.position).normalized;
            Vector2 force = dir * Speed;

            _rigidbody.AddForce(force * Time.fixedDeltaTime);

            float dist = Vector2.Distance(_path.vectorPath[_curPoint], transform.position);

            if (dist < 0.5f)
            {
                _curPoint++;
            }
        }
    }
}

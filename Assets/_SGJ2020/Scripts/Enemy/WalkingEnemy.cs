using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BulletShooter))]
public class WalkingEnemy : Enemy
{

    public float moveVelocity = 6f;
    public float maxSpeed = 13f;
    public float shootCooldown = 3f;
    public float shootForce = 5f;
    private BulletShooter _bulletShooter;
    private float _shootTimer = 0f;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        _bulletShooter = GetComponent<BulletShooter>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_shootTimer > 0)
        {
            _shootTimer -= Time.deltaTime;
        }

        if (_player)
        {
            Vector2 result = Vector2.zero;

            Vector2 playerPosition = _player.transform.position;
            Vector2 diff = playerPosition - (Vector2)transform.position;

            result += (Vector2)transform.right * (diff.x * moveVelocity) * Time.fixedDeltaTime;
            if(Mathf.Abs(result.x) > maxSpeed) {
                result.x = Mathf.Sign(result.x) * maxSpeed;
            }

             _rb.velocity = result;

            if (_shootTimer <= 0f)
            {
                var target = Physics2D.Raycast((Vector2)transform.position + diff.normalized, diff.normalized, 15);

                Debug.Log(target.collider);
                if (!target.collider || target.collider.tag == "Player")
                {
                    _bulletShooter.Shoot((Vector2)transform.position + diff.normalized, diff * shootForce);
                    _shootTimer = shootCooldown;
                }
            }
        }
    }
}

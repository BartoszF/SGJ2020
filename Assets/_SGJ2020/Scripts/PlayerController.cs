using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    public Transform rayDownOrigin;

    public Transform rayLeftOrigin, rayRightOrigin;

    public SpriteRenderer playerSprite;

    public float jumpVelocity = 5f;
    public Vector2 wallJumpVelocity = new Vector2(5, 5);

    public float horizontalVelocity = 10f;

    public float maxSpeed = 5f;
    public float slowedMaxSpeed = 2f;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public float damagedBySpikesColddown = 1f;
    private bool _damagedBySpikes = false;
    private float _currentDamagedBySpikesColddown = 0;

    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;

    private bool _jumped = false;

    public void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
    }

    public void FixedUpdate()
    {
        Vector2 result = _rigidbody2D.velocity;
        var floor = Physics2D.Raycast(rayDownOrigin.position, -transform.up, 0.05f);
        var isOnFloor = floor.collider && floor.collider.CompareTag("Ground");

        if (_jumped && isOnFloor)
        {
            _jumped = false;
        }

        var right = transform.right;
        var leftWall = (Physics2D.Raycast(rayLeftOrigin.position, -right, 0.1f));
        var rightWall = Physics2D.Raycast(rayRightOrigin.position, right, 0.1f);

        bool isOnLeftWall = leftWall.collider && leftWall.collider.CompareTag("Ground");
        bool isOnRightWall = rightWall.collider && rightWall.collider.CompareTag("Ground");


        if (isOnFloor && Input.GetButton("Jump"))
        {
            result += (Vector2) transform.up * jumpVelocity;
        }

        if (!_jumped && !isOnFloor && isOnLeftWall && Input.GetButton("Jump"))
        {
            Vector2 direction = new Vector2();
            direction += (Vector2) transform.right * wallJumpVelocity.x;
            direction += (Vector2) transform.up * wallJumpVelocity.y;
            _jumped = true;
            result += direction;
        }

        if (!_jumped && !isOnFloor && isOnRightWall && Input.GetButton("Jump"))
        {
            Vector2 direction = new Vector2();
            direction += -(Vector2) transform.right * wallJumpVelocity.x;
            direction += (Vector2) transform.up * wallJumpVelocity.y;
            _jumped = true;
            result += direction;
        }


        if (Math.Abs(Input.GetAxis("Horizontal")) > 0.1f)
        {
            result += (Vector2) transform.right *
                      (Input.GetAxis("Horizontal") * horizontalVelocity * Time.fixedDeltaTime);
        }
        else if (isOnFloor)
        {
            result.x *= 0.3f * Time.fixedDeltaTime;
        }


        if (_rigidbody2D.velocity.y < 0)
        {
            result += (Vector2) transform.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime);
        }
        else if (_rigidbody2D.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            result += (Vector2) transform.up * (Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime);
        }

        var currentMaxSpeed = _damagedBySpikes ? slowedMaxSpeed : maxSpeed;
        if (Math.Abs(result.x) > currentMaxSpeed)
        {
            result.x = Math.Sign(result.x) * currentMaxSpeed;
        }

        _rigidbody2D.velocity = result;
        _currentDamagedBySpikesColddown -= Time.deltaTime;
        if (_currentDamagedBySpikesColddown < -0)
        {
            _damagedBySpikes = false;
        }

        playerSprite.color = _damagedBySpikes ? Color.red : Color.white;
    }

    public void DamageBySpikes()
    {
        _currentDamagedBySpikesColddown = damagedBySpikesColddown;
        _damagedBySpikes = true;
    }

    public void SetDrunk(bool drunk)
    {
        // TODO
    }

    public void SetNoShooting(bool noShooting)
    {
        // TODO
    }

    public void SetNoHealing(bool noHealing)
    {
        // TODO
    }
}
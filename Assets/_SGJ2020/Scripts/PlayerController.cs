using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    public Transform rayDownOrigin;

    public Transform rayLeftOrigin, rayRightOrigin;
    public float jumpVelocity = 5f;
    public Vector2 wallJumpVelocity = new Vector2(5,5);

    public float horizontalVelocity = 10f;

    public float maxSpeed = 5f;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;

    public void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
    }

    public void FixedUpdate()
    {
        Vector2 result = _rigidbody2D.velocity;
        var isOnFloor = Physics2D.Raycast(rayDownOrigin.position, -transform.up, 0.05f);

        var isOnLeftWall = Physics2D.Raycast(rayLeftOrigin.position, -transform.right, 0.1f);
        var isOnRightWall = Physics2D.Raycast(rayRightOrigin.position, transform.right, 0.1f);


        if (isOnFloor.collider && Input.GetButton("Jump"))
        {
            result += (Vector2)transform.up * jumpVelocity;
        }

        if (isOnLeftWall.collider && Input.GetButton("Jump"))
        {
            Vector2 direction = new Vector2();
            direction += (Vector2)transform.right * wallJumpVelocity.x;
            direction += (Vector2)transform.up *wallJumpVelocity.y;
            result += direction;
        }
        if (isOnRightWall.collider && Input.GetButton("Jump"))
        {
            Vector2 direction = new Vector2();
            direction += -(Vector2)transform.right * wallJumpVelocity.x;
            direction += (Vector2)transform.up *wallJumpVelocity.y;
        }

        if(Math.Abs(Input.GetAxis("Horizontal")) > 0.1f) {
            result += (Vector2)transform.right * (Input.GetAxis("Horizontal") * horizontalVelocity) * Time.fixedDeltaTime;
        } else {
            //result.x *= 0.3f * Time.fixedDeltaTime;
        }


        if (_rigidbody2D.velocity.y < 0)
        {
            result += (Vector2)transform.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (_rigidbody2D.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            result += (Vector2)transform.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

        if(Math.Abs(result.x) > maxSpeed) {
            result.x = Math.Sign(result.x) * maxSpeed;
        }

        _rigidbody2D.velocity = result;
    }
}

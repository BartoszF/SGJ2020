using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    public Transform rayOrigin;
    public float jumpVelocity = 5f;

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
        var isOnFloor = Physics2D.Raycast(rayOrigin.position, Vector2.down, 0.05f);

        if (isOnFloor.collider /*&& _collider2D.IsTouching(new ContactFilter2D())*/ && Input.GetButton("Jump"))
        {
            result += Vector2.up * jumpVelocity;
        }

        if(Math.Abs(Input.GetAxis("Horizontal")) > 0.1f) {
            result += Vector2.right * (Input.GetAxis("Horizontal") * horizontalVelocity) * Time.fixedDeltaTime;
        } else {
            result.x *= 0.3f * Time.fixedDeltaTime;
        }


        if (_rigidbody2D.velocity.y < 0)
        {
            result += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (_rigidbody2D.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            result += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

        if(Math.Abs(result.x) > maxSpeed) {
            result.x = Math.Sign(result.x) * maxSpeed;
        }

        _rigidbody2D.velocity = result;
    }
}

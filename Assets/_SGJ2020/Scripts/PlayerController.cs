using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    public Transform rayDownLeftOrigin, rayDownRightOrigin;

    public Transform rayLeftOrigin, rayRightOrigin;
    public float jumpVelocity = 5f;
    public Vector2 wallJumpVelocity = new Vector2(5, 5);

    public float horizontalVelocity = 10f;

    public float maxSpeed = 5f;
    public float maxHorizontalAirSpeed = 10f;
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
        var floorLeft = Physics2D.Raycast(rayDownLeftOrigin.position, -transform.up, 0.05f);
        var floorRight = Physics2D.Raycast(rayDownRightOrigin.position, -transform.up, 0.05f);

        var isOnFloor = (floorLeft.collider && floorLeft.collider.tag == "Ground") || (floorRight.collider && floorRight.collider.tag == "Ground");

        var leftWall = (Physics2D.Raycast(rayLeftOrigin.position, -transform.right, 0.1f));
        var rightWall = Physics2D.Raycast(rayRightOrigin.position, transform.right, 0.1f);

        bool isOnLeftWall = leftWall.collider && leftWall.collider.tag == "Ground";
        bool isOnRightWall = rightWall.collider && rightWall.collider.tag == "Ground";


        if (isOnFloor && Input.GetButton("Jump"))
        {
            result += (Vector2)transform.up * jumpVelocity;
        }

        if (!isOnFloor && isOnLeftWall && Input.GetButton("Jump"))
        {
            Vector2 direction = new Vector2();
            direction += (Vector2)transform.right * wallJumpVelocity.x;
            direction += (Vector2)transform.up * wallJumpVelocity.y;
            result += direction;
        }
        if (!isOnFloor && isOnRightWall && Input.GetButton("Jump"))
        {
            Vector2 direction = new Vector2();
            direction += -(Vector2)transform.right * wallJumpVelocity.x;
            direction += (Vector2)transform.up * wallJumpVelocity.y;
            result += direction;
        }

        if (Math.Abs(Input.GetAxis("Horizontal")) > 0.1f)
        {
            result += (Vector2)transform.right * (Input.GetAxis("Horizontal") * horizontalVelocity) * Time.fixedDeltaTime;
        }
        else if (isOnFloor)
        {
            result.x *= 0.2f * Time.fixedDeltaTime;
        }


        if (_rigidbody2D.velocity.y < 0)
        {
            result += (Vector2)transform.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (_rigidbody2D.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            result += (Vector2)transform.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

        if (isOnFloor)
        {
            if (Math.Abs(result.x) > maxSpeed)
            {
                result.x = Math.Sign(result.x) * maxSpeed;
            }
        } else {
            if (Math.Abs(result.x) > maxHorizontalAirSpeed)
            {
                result.x = Math.Sign(result.x) * maxHorizontalAirSpeed;
            }
        }

        _rigidbody2D.velocity = result;
    }
}

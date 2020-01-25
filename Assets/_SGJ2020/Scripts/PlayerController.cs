using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    public float jumpForce = 100f;
    public float horizontalForce = 100f;
    
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;

    public void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
    }

    public void FixedUpdate()
    {
        var isOnFloor = Physics2D.Raycast(transform.position, Vector2.down, 0.9f);
        if (isOnFloor.collider && _collider2D.IsTouching(new ContactFilter2D()) && Input.GetAxis("Jump") > 0)
        {
            _rigidbody2D.AddForce(Vector2.up * jumpForce);            
        }
        _rigidbody2D.AddForce(Vector2.right * (Input.GetAxis("Horizontal") * horizontalForce));
    }
}

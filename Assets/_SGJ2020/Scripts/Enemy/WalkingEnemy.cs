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

    private JumpNode targetNode;
    private Vector2 startJumpPosition, midJumpPosition;
    private float jumpTimer = 0f;
    private bool isJumping = false;

    void Start()
    {
        base.Start();
        _bulletShooter = GetComponent<BulletShooter>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 result = Vector2.zero;
        if (_shootTimer > 0)
        {
            _shootTimer -= Time.deltaTime;
        }

        if (_player)
        {
            Vector2 playerPosition = _player.transform.position;
            Vector2 diff = playerPosition - (Vector2)transform.position;

            result += (Vector2)transform.right * (diff.x * moveVelocity) * Time.fixedDeltaTime;
            if (Mathf.Abs(result.x) > maxSpeed)
            {
                result.x = Mathf.Sign(result.x) * maxSpeed;
            }

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
        else
        {
            if (targetNode == null)
            {
                JumpNode closest = null;
                float minDistance = float.PositiveInfinity;
                foreach (JumpNode node in PlatformPathfinding.nodes)
                {
                    float dist = Vector2.Distance(node.transform.position, transform.position);
                    if (dist < minDistance)
                    {
                        closest = node;
                        minDistance = dist;
                    }
                }

                targetNode = closest;
            }
            else
            {
                if (Vector2.Distance(targetNode.transform.position, transform.position) < 0.5f)
                {

                    int index = Enemy.rnd.Next(targetNode.connectedNodes.Count);
                    targetNode = targetNode.connectedNodes[index];

                    isJumping = false;
                    jumpTimer = 0f;
                }
                else
                {
                    if (!isJumping && Mathf.Abs(targetNode.transform.position.y - transform.position.y) < 1f)
                    {
                        Vector2 diff = (Vector2)targetNode.transform.position - (Vector2)transform.position;

                        result += (Vector2)transform.right * (diff.normalized.x * moveVelocity) * Time.fixedDeltaTime;
                        if (Mathf.Abs(result.x) > maxSpeed)
                        {
                            result.x = Mathf.Sign(result.x) * maxSpeed;
                        }
                    }
                    else
                    {
                        if (!isJumping)
                        {
                            jumpTimer = 0;
                            startJumpPosition = transform.position;
                            midJumpPosition = (startJumpPosition + (Vector2)targetNode.transform.position) / 2;
                            midJumpPosition.y = targetNode.transform.position.y > startJumpPosition.y ? targetNode.transform.position.y + 2 : startJumpPosition.y+2;
                            isJumping = true;

                        }
                        if (jumpTimer >= 1f)
                        {
                            isJumping = false;
                        }

                        if (jumpTimer <= 1f)
                        {
                            Vector2 endPosition = targetNode.transform.position;
                            Vector2 lerped = cubeBezier2(startJumpPosition, midJumpPosition, midJumpPosition, endPosition, jumpTimer);
                            Vector2 diff = lerped - (Vector2)transform.position;
                            _rb.position = lerped;
                            jumpTimer += Time.deltaTime;
                        }

                    }
                }
            }
        }

        _rb.velocity = result;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        if (midJumpPosition != null)
        {
            Gizmos.DrawSphere(midJumpPosition, 0.3f);
        }

        Gizmos.color = Color.green;
        if (startJumpPosition != null)
        {
            Gizmos.DrawSphere(startJumpPosition, 0.3f);
        }

        Gizmos.color = Color.white;
        if (targetNode != null)
        {
            Gizmos.DrawSphere(targetNode.transform.position, 0.5f);
        }
    }

    public Vector2 cubeBezier2(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
 {
     float r = 1f - t;
     float f0 = r * r * r;
     float f1 = r * r * t * 3;
     float f2 = r * t * t * 3;
     float f3 = t * t * t;

     return new Vector2(
     f0*p0.x + f1*p1.x + f2*p2.x + f3*p3.x,
     f0*p0.y + f1*p1.y + f2*p2.y + f3*p3.y
 );
 }
}

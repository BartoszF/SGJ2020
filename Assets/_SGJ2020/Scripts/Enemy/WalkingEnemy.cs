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
            Debug.Log("NODE");
            if (targetNode == null)
            {
                Debug.Log("NO NODE");
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
                Debug.Log("TARGET " + targetNode);
            }
            else
            {
                if (Vector2.Distance(targetNode.transform.position, transform.position) < 0.5f)
                {
                    Debug.Log("Next NODE");
                    int index = Enemy.rnd.Next(targetNode.connectedNodes.Count);
                    targetNode = targetNode.connectedNodes[index];
                }
                else
                {
                    if (Mathf.Abs(targetNode.transform.position.y - transform.position.y) < 1f)
                    {
                        Debug.Log("SAME Y");
                        Vector2 diff = (Vector2)targetNode.transform.position - (Vector2)transform.position;

                        result += (Vector2)transform.right * (diff.normalized.x * moveVelocity) * Time.fixedDeltaTime;
                        if (Mathf.Abs(result.x) > maxSpeed)
                        {
                            result.x = Mathf.Sign(result.x) * maxSpeed;
                        }
                    } else {
                        Debug.Log("JUMP");
                        if(!isJumping) {
                            jumpTimer = 0;
                            startJumpPosition = transform.position;
                            midJumpPosition = (startJumpPosition + (Vector2)targetNode.transform.position)/2;
                            midJumpPosition.y = targetNode.transform.position.y + 2;
                            isJumping = true;

                        }
                        if(jumpTimer >= 1f) {
                            isJumping = false;
                        } 

                        if(jumpTimer <= 1f) {
                            Vector2 target = Lerp3(startJumpPosition, midJumpPosition,targetNode.transform.position, jumpTimer);
                            Vector2 diff = target - (Vector2)transform.position;
                            result = diff * moveVelocity;
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

        if(midJumpPosition != null) {
            Gizmos.DrawSphere(midJumpPosition,0.3f);
        }    

        Gizmos.color = Color.green;
        if(startJumpPosition != null) {
            Gizmos.DrawSphere(startJumpPosition, 0.3f);
        }

        Gizmos.color = Color.white;
        if(targetNode != null) {
            Gizmos.DrawSphere(targetNode.transform.position, 0.5f);
        }
    }

    Vector2 Lerp3(Vector2 a, Vector2 b, Vector2 c, float t) {
        if (t <= 0.5f)
        {
            return Vector2.Lerp(a, b, t * 2f);
        }
        else
        {
            return Vector2.Lerp(b, c, t);
        }
    }
}

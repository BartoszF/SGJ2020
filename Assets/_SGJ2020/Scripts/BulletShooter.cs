using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    public GameObject bulletPrefab;

    public void Shoot(Vector2 position, Vector2 velocity)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        Rigidbody2D brb = bullet.GetComponent<Rigidbody2D>();
        brb.AddForce(velocity, ForceMode2D.Impulse);
    }

}

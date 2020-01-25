using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingController : MonoBehaviour
{

    public BulletShooter bulletShooter;
    public float radius = 1f;
    public float shootForce = 10f;

    void Update()
    {
        if(Input.GetButtonDown("Fire1")) {
            Vector3 mouse = Input.mousePosition;
            Debug.Log(mouse);
            Vector3 viewportMouse = Camera.main.ScreenToWorldPoint(mouse);
            Debug.Log(viewportMouse);

            Vector2 diff = viewportMouse - transform.position;
            Vector2 normalized = diff.normalized * radius;

            bulletShooter.Shoot((Vector2)transform.position + normalized, diff.normalized * shootForce);
        }
    }
}

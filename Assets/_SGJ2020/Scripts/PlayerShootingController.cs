using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingController : MonoBehaviour
{

    public GameObject bulletPrefab;
    public float radius = 1f;
    public float shootForce = 10f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")) {
            Vector3 mouse = Input.mousePosition;
            Debug.Log(mouse);
            Vector3 viewportMouse = Camera.main.ScreenToWorldPoint(mouse);
            Debug.Log(viewportMouse);

            Vector2 diff = viewportMouse - transform.position;
            Vector2 normalized = diff.normalized * radius;

            GameObject bullet = Instantiate(bulletPrefab, (Vector2)transform.position + normalized, Quaternion.identity);
            Rigidbody2D brb = bullet.GetComponent<Rigidbody2D>();
            brb.AddForce(diff.normalized * shootForce, ForceMode2D.Impulse);
        }
    }
}

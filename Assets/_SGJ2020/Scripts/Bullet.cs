using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public GameObject bulletExplosion;

    void OnCollisionEnter2D(Collision2D other) {
        GameObject explosion = Instantiate(bulletExplosion,transform.position, Quaternion.identity);
        Destroy(explosion,15);
        Destroy(this.gameObject);
    }
}

using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bulletExplosion;

    private void OnCollisionEnter2D(Collision2D other) {
        var explosion = Instantiate(bulletExplosion,transform.position, Quaternion.identity);
        Destroy(explosion,5);
        Destroy(gameObject);
    }
}

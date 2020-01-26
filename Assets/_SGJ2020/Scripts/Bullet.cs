using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bulletExplosion;
    public AudioClip ExplosionSound;

    private void OnCollisionEnter2D(Collision2D other) {
        var explosion = Instantiate(bulletExplosion,transform.position, Quaternion.identity);
        AudioSource audio = explosion.AddComponent(typeof(AudioSource)) as AudioSource;
        audio.PlayOneShot(ExplosionSound);
        
        Destroy(explosion,5);
        Destroy(gameObject);
    }
}

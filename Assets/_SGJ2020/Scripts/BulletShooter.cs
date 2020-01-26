using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject brokenBulletPrefab;

    public AudioClip ShootSound;

    private AudioSource _audioSource;

    private PlayerController _controller;
    public void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void Shoot(Vector2 position, Vector2 velocity)
    {
        if (_controller && _controller.shootingDisabled)
        {
            var brokenBullet = Instantiate(brokenBulletPrefab, position, Quaternion.identity);
            var bbrb = brokenBullet.GetComponent<Rigidbody2D>();
            bbrb.AddForce(velocity / 2f, ForceMode2D.Impulse);
            return;
        }
        _audioSource.PlayOneShot(ShootSound);
        var bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        var brb = bullet.GetComponent<Rigidbody2D>();
        brb.AddForce(velocity, ForceMode2D.Impulse);
    }

}

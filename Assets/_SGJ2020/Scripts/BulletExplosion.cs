using System;
using UnityEngine;

namespace _SGJ2020.Scripts
{
    [RequireComponent(typeof(Collider2D))]
    public class BulletExplosion : MonoBehaviour
    {
        private float _timeAlive;

        public void Update()
        {
            _timeAlive += Time.deltaTime;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (_timeAlive > 0.2f)
            {
                return;
            }
            Debug.Log(other.name);
            var player = other.CompareTag("Player") ? other : null;
            if (player != null)
            {
                player.GetComponent<PlayerController>().DamageByBullet();
            }

            if (other.CompareTag("Mob"))
            {
                other.GetComponent<Enemy>().Kill();
            }
            if (other.CompareTag("FlyingMob"))
            {
                other.GetComponent<FlyingEnemy>().Kill();
            }
        }
    }
}
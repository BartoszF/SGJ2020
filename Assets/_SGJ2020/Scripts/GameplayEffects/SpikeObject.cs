using System;
using UnityEngine;

namespace _SGJ2020.Scripts.GameplayEffects
{
    [RequireComponent(typeof(Collider2D))]
    public class SpikeObject : MonoBehaviour
    {
        public float enablingTime = 1f;
        public float damageColddown = 1f;

        private State _state = State.Disabled;
        private float _progress;
        private float _currentDamageColddown;

        private Vector3 _originalScale;

        public void Awake()
        {
            _originalScale = transform.localScale;
        }

        private void Update()
        {
            switch (_state)
            {
                case State.Enabling:
                    _progress += Time.deltaTime;
                    if (_progress >= enablingTime)
                    {
                        _state = State.Enabled;
                    }

                    break;
                case State.Disabling:
                    _progress -= Time.deltaTime;
                    if (_progress <= 0f)
                    {
                        _state = State.Disabled;
                    }

                    break;
                case State.Disabled:
                    break;
                case State.Enabled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _currentDamageColddown -= Time.deltaTime;

            transform.localScale = _originalScale * _progress / enablingTime;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            var playerController = other.GetComponent<PlayerController>();
            if (playerController == null || !(_currentDamageColddown <= 0) || _state != State.Enabled) return;
            _currentDamageColddown = damageColddown;
            playerController.DamageBySpikes();
            Debug.Log("playerDamaged");
        }

        public void SetEnabled(bool spikes)
        {
            if (spikes && _state == State.Disabled)
            {
                _state = State.Enabling;
            }
            else if (!spikes && _state == State.Enabled)
            {
                _state = State.Disabling;
            }
        }

        private enum State
        {
            Enabling,
            Disabling,
            Disabled,
            Enabled
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _SGJ2020.Scripts.GameplayEffects
{
    public class GameplayEffectManager : MonoBehaviour
    {
        public bool reverseGravity = false;
        public bool drunk = false;
        public bool noShooting = false;
        public bool heating = false;
        public bool spikes = false;

        private List<SpikeObject> _spikeObjects;
        private PlayerController _player;

        public void Awake()
        {
            _player = GameObject.FindGameObjectsWithTag("Player").First().GetComponent<PlayerController>();
            _spikeObjects = FindObjectsOfType<SpikeObject>().ToList();
            Debug.Log(_spikeObjects.Count);
        }

        private void ClearAll()
        {
            reverseGravity = false;
            drunk = false;
            noShooting = false;
            heating = false;
            spikes = false;
        }

        public void Update()
        {
            Physics2D.gravity = (reverseGravity ? Vector2.up : Vector2.down) * 9.81f;
            _spikeObjects.ForEach(s => s.SetEnabled(spikes));
            _player.SetDrunk(drunk);
            _player.SetNoShooting(noShooting);
        }

        private void ToggleRandomEffects()
        {
            
        }
    }
}
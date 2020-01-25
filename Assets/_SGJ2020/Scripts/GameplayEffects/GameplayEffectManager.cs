using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _SGJ2020.Scripts.GameplayEffects
{
    public class GameplayEffectManager : MonoBehaviour
    {
        public float timeBetweenWaves = 10f;
        private float _currentTimeFromWave = 0f;
        public TextMeshProUGUI waveCountdown;
        private int _nextWaveSize = 2;
        
        public GameObject groundSpawners;
        public GameObject flyingSpawners;
        public GameObject flyingEnemyPrefab;
        public GameObject groundEnemyPrefab;
        
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

        public void Update()
        {
            Physics2D.gravity = (reverseGravity ? Vector2.up : Vector2.down) * 9.81f;
            _spikeObjects.ForEach(s => s.SetEnabled(spikes));
            _player.SetDrunk(drunk);
            _player.SetNoShooting(noShooting);

            _currentTimeFromWave += Time.deltaTime;
            if (_currentTimeFromWave > timeBetweenWaves)
            {
                _currentTimeFromWave = 0;
                SpawnMobs();
                _nextWaveSize++;
                ToggleRandomEffects();
            }
            waveCountdown.text = "" + (timeBetweenWaves - _currentTimeFromWave).ToString("00.00");
        }

        private void SpawnMobs()
        {
            for (var i = 0; i < _nextWaveSize; i++)
            {
                if (Random.value < 0.5f)
                {
                    var spawnerCount = flyingSpawners.transform.childCount;
                    var randomIndex = Random.Range(0, spawnerCount);
                    var newEnemy = Instantiate(flyingEnemyPrefab);
                    newEnemy.transform.position = flyingSpawners.transform.GetChild(randomIndex).position;
                }
                else
                {
                    var spawnerCount = groundSpawners.transform.childCount;
                    var randomIndex = Random.Range(0, spawnerCount);
                    var newEnemy = Instantiate(groundEnemyPrefab);
                    newEnemy.transform.position = groundSpawners.transform.GetChild(randomIndex).position;
                }
            }
        }

        private void ToggleRandomEffects()
        {
            var rand = Random.value;
            reverseGravity = rand.IsBetween(0f, 0.2f);
            drunk = rand.IsBetween(0.2f, 0.4f);
            noShooting = rand.IsBetween(0.4f, 0.6f);
            heating = rand.IsBetween(0.6f, 0.8f);
            spikes = rand.IsBetween(0.8f, 1f);
        }

    }
}
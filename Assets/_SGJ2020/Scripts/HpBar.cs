using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _SGJ2020.Scripts
{
    public class HpBar : MonoBehaviour
    {
        private PlayerController _playerController;
        public List<GameObject> hearts;

        public void Awake()
        {
            var playerGO = GameObject.FindGameObjectsWithTag("Player").First();
            _playerController = playerGO.GetComponent<PlayerController>();
        }

        public void Update()
        {
            for (var i = 0; i < hearts.Count; i++)
            {
                hearts[i].SetActive(i < _playerController.CurrentHealth);
            }
        }
    }
}
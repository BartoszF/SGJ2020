using System;
using UnityEngine;

namespace _SGJ2020.Scripts
{
    [RequireComponent(typeof(Collider2D))]
    public class Killzone : MonoBehaviour
    {
        public void Update()
        {

            StateHolder.State.CurrentScreen = GameScreen.GameOver;
        }
    }
}
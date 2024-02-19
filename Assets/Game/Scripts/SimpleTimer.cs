using System;
using Game.Scripts.Game.States;
using Game.Scripts.Patterns;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    public class SimpleTimer : MonoBehaviour
    {
        private float _timer;

        private bool _isCounting;
        
        // Start is called before the first frame update
        private void Start()
        {
            EventBus<GameStates>.Subscribe(GameStates.Paused, TimerPause);
            EventBus<GameStates>.Subscribe(GameStates.Running, TimerResume);
        }

        private void OnDestroy()
        {
            EventBus<GameStates>.Unsubscribe(GameStates.Paused, TimerPause);
            EventBus<GameStates>.Unsubscribe(GameStates.Running, TimerResume);
        }

        // Update is called once per frame
        private void Update()
        {
            if (!_isCounting) return;
            _timer += Time.deltaTime;
        }

        private void TimerPause()
        {
            _isCounting = false;
            Debug.Log($"Paused timer at: {_timer}");
        }
        
        private void TimerResume()
        {
            _isCounting = true;
            Debug.Log($"Resumed timer at: {_timer}");
        }
    }
}

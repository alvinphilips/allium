using System.Collections.Generic;
using Game.Scripts.Fusion;
using Game.Scripts.Patterns;
using Game.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Game.Scripts.Game.States
{
    public class PlayState : IState<GameManager>
    {
        private readonly List<ARRaycastHit> _arHits = new();
        
        public void OnStateEnter(GameManager state)
        {
            EventBus<GameStates>.Publish(GameStates.Running);
            MenuManager.Instance.HideDummy();
        }
    
        public void OnUpdate(GameManager state)
        {
            var shouldSpawn = false;
            var spawnPosition = Vector3.zero;
            var spawnRotation = Quaternion.identity;
            
            if (state.IsAREnabled && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (!state.arRaycastManager.Raycast(Input.GetTouch(0).position, _arHits, TrackableType.PlaneWithinPolygon))
                {
                    return;
                }

                shouldSpawn = true;
                spawnPosition = _arHits[0].pose.position;
                spawnRotation = _arHits[0].pose.rotation;

                foreach (var plane in state.arPlaneManager.trackables)
                {
                    plane.gameObject.SetActive(false);
                }

                state.arPlaneManager.enabled = false;
            } else if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            }
            
            if (!shouldSpawn) return;
            
            state.SpawnItem(spawnPosition, spawnRotation);
        }
        
        public void OnStateExit(GameManager state)
        {
            EventBus<GameStates>.Publish(GameStates.Paused);
        }
    }
}

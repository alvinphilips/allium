using Fusion;
using Game.Scripts.Game;
using UnityEngine;

namespace Game.Scripts.Resources
{
    public class Resource: MonoBehaviour
    {
        public ResourceType Type { get; set; }
        public int Amount { get; set; }
    }
}
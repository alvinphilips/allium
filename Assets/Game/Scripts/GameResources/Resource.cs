using Fusion;
using Game.Scripts.Game;
using Game.Scripts.Patterns;
using UnityEngine;

namespace Game.Scripts.GameResources
{
    public class Resource: MonoBehaviour, IVisitable<Resource>
    {
        public ResourceType Type { get; set; }
        public int Amount { get; set; }
        public void Accept(IVisitor<Resource> visitor)
        {
            visitor.Visit(this);
        }
    }
}
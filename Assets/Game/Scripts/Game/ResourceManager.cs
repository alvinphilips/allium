using Game.Scripts.Game;
using Game.Scripts.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Game
{
    public enum ResourceType
    {
        Money,
        Troops  //etc to be determined and added
    }

    public class ResourceManager : Singleton<ResourceManager>
    {
        private Dictionary<ResourceType, int> _resources;

        private int Money
        {
            get => _resources[ResourceType.Money];
            set => _resources[ResourceType.Money] = value;
        }
        
        private int Troops
        {
            get => _resources[ResourceType.Troops];
            set => _resources[ResourceType.Troops] = value;
        }
        
        public int GetMoney() { return Money; }

        public void AddMoney(int val)
        {
            Money += val;
        }

        public int GetTroops() { return Troops; }

        public void AddTroops()
        {
            Troops++;
        }
    }
}
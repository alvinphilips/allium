using Game.Scripts.Game;
using Game.Scripts.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Game
{
    public enum Resources
    {
        Money,
        Troops  //etc to be determined and added
    }

    public class ResourceManager : Singleton<ResourceManager>
    {
        private Dictionary<Resources, int> _resources;

        private int Money
        {
            get => _resources[Resources.Money];
            set => _resources[Resources.Money] = value;
        }
        
        private int Troops
        {
            get => _resources[Resources.Troops];
            set => _resources[Resources.Troops] = value;
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
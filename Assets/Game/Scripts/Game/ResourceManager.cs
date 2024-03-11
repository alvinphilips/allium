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
        int Money = 0;
        int Troops = 0;

        public int GetMoney() { return Money; }

        public void AddMoney(int val)
        {
            Money += val;
        }

        public int GetTroops() { return Money; }

        public void AddTroops()
        {
            Troops++;
        }
    }
}
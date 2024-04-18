using System;
using UnityEngine;

namespace Game.Scripts.Troops
{
    [Serializable]
    public enum UnitType
    {
        Looters,
        Footmen
    }

    public class Unit : Troop
    {
        //To decide on which action to perform
        [SerializeField]
        public UnitType unitType;

        

        public override Transform GetTarget()
        {
            Target = PlacementHandler.Instance.GetClosestTarget(transform.position, ObjectType.ResourceBldg, Range).transform;

            return Target;
        }

        public override void Aim(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public void Loot()
        {
            //Action of looting resource to be performed
        }

        public override void Fire()
        {
            Debug.Log("Firing");

            //Firing logic to be placed
        }
    }
}
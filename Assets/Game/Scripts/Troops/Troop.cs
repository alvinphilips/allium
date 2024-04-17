using Fusion;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Troops
{
    public class Troop : NetworkBehaviour, IDestroyable
    {
        public Transform Target;

        [Networked] public int Range { get; set; }

        [Networked] public int Damage { get; set; }

        public float turretRotateSpeed = 10f;

        //Angle within which turret can fire
        public float fireThreshold;
        public float DamageMultiplier { get; set; }
        public GameObject Owner { get; set; }

        public float fireDelay;

        UnityEvent<GameObject> IDestroyable.OnObjectDestroyed { get; set; }
        public float Health { get; set; }

        protected virtual void Start()
        {
            Health = 50;
            DamageMultiplier = 1;
            Owner = gameObject;
        }

        void IDestroyable.Damage(float damage)
        {
            Health -= damage * DamageMultiplier;
            if (Health <= 0)
                ((IDestroyable)this).DestroyObject();
        }

        void IDestroyable.DestroyObject()
        {
            ((IDestroyable)this).OnObjectDestroyed?.Invoke(Owner);
            GameObject.Destroy(Owner);
        }

        public virtual Transform GetTarget()
        {
            return Target;
        }

        public virtual void Aim(Quaternion rotation)
        {
       
        }
    
        public virtual void Fire()
        {
        
        }
    }
}

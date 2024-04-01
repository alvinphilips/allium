using Fusion;
using System;
using UnityEngine;

namespace Game.Scripts.Buildings
{
    [Serializable]
    public struct Bullet
    {
        public float Velocity { get; set; }
        public float Damage { get; set; }
    }
    
    public class SentryGun : NetworkBehaviour
    {
        [SerializeField, Unit(Units.PerSecond)] private float fireRate;
        [SerializeField, Unit(Units.Count)] private int beltSize = 300;
        [SerializeField] private Bullet ammunition;
        [SerializeField, Unit(Units.DegreesPerSecond)] private float turretMaxRotationSpeed;
        
        public float FireRate
        {
            get => fireRate;
            set => fireRate = value;
        }

        public float FireDelay => 1f / fireRate;
        
        public Bullet Ammunition { 
            get => ammunition; 
            set => ammunition = value;
        }

        public int BeltSize
        {
            get => beltSize; 
            set => beltSize = value;
        }

        public float TurretMaxRotationSpeed
        {
            get => turretMaxRotationSpeed; 
            set => turretMaxRotationSpeed = value;
        }
    }
}

using Fusion;
using System;
using Game.Scripts.Audio;
using UnityEngine;

namespace Game.Scripts.Buildings
{
    [Serializable]
    public enum BulletType
    {
        HitScan,
        Projectile
    }
    
    [Serializable]
    public struct Bullet
    {
        public float Velocity { get; set; }
        public float Damage { get; set; }
        public float Range { get; set; }
        public BulletType Type { get; set; }
    }
    
    public class SentryGun : DefenceTower
    {
        [SerializeField, Unit(Units.PerSecond)] private float fireRate;
        [SerializeField, Unit(Units.Count)] private int beltSize = 300;
        [SerializeField] private Bullet ammunition;
        [SerializeField, Unit(Units.DegreesPerSecond)] private float turretMaxRotationSpeed;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletSpawnPosition;
        [SerializeField] private Transform turret;
        [SerializeField] private Transform turretBase;
        [SerializeField] private AudioObject fireAudio;
        [SerializeField] private LayerMask targetLayerMask;
        [SerializeField, Unit(Units.Seconds)] private float reloadDelay;

        private bool _audioEnabled;
        private float _fireDelayTimer;
        private IDestroyable _target = null;
        private float _currentRoundsInBelt;
        
        public float FireRate
        {
            get => fireRate;
            set => fireRate = value;
        }

        public float ReloadDelay
        {
            get => reloadDelay;
            set => reloadDelay = value;
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

        public void Start()
        {
            if (fireAudio != null)
            {
                AudioManager.Instance.LoadSfx(fireAudio);
                _audioEnabled = true;
            }

            _currentRoundsInBelt = BeltSize;
        }

        public override void FixedUpdateNetwork()
        {
            if (!Object.HasStateAuthority) return;

            _fireDelayTimer -= FireDelay;

            if (_target != null && _target.IsDead)
            {
                _target = null;
            }
            
            FindClosestTarget();
            
            if (_target != null && _fireDelayTimer < 0)
            {
                Fire();
            }
        }

        private void FindClosestTarget()
        {
            if (_target != null) return;

            var targets = Physics.OverlapSphere(transform.position, Ammunition.Range, targetLayerMask);
            (Collider, float) closestTarget = (null, float.MaxValue);
            foreach (var target in targets)
            {
                // Skip anything that cannot be destroyed
                if (!target.TryGetComponent(out IDestroyable _)) return;
                
                var distanceToTarget = Vector3.Distance(bulletSpawnPosition.position, target.transform.position);
                if (distanceToTarget < closestTarget.Item2)
                {
                    closestTarget = (target, distanceToTarget);
                }
            }

            if (closestTarget.Item1 == null) return;

            if (closestTarget.Item1.TryGetComponent(out IDestroyable destroyable))
            {
                _target = destroyable;
            }
        }

        private void Fire()
        {
            switch (Ammunition.Type)
            {
                case BulletType.Projectile:
                    if (Runner.Spawn(bulletPrefab, bulletSpawnPosition.position, bulletSpawnPosition.rotation)
                        .TryGetComponent<Projectile>(out var projectile))
                    {
                        projectile.Velocity = Ammunition.Velocity;
                        projectile.Damage = Ammunition.Damage;
                        projectile.Range = Ammunition.Range;
                    }
                    break;
                case BulletType.HitScan:
                    PerformHitScan();
                    break;
            }

            RpcFireEffects();
            _currentRoundsInBelt--;
            _fireDelayTimer = FireDelay;
            if (_currentRoundsInBelt <= 0)
            {
                ReloadBelt();
            } 
        }

        private void PerformHitScan()
        {
            if (!Physics.Raycast(bulletSpawnPosition.position, bulletSpawnPosition.forward, out var hitInfo,
                    Ammunition.Range, targetLayerMask)) return;
            
            if (hitInfo.collider.gameObject.TryGetComponent(out _target))
            {
                _target.Damage(Ammunition.Damage);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RpcFireEffects()
        {
            if (_audioEnabled)
            {
                AudioManager.Instance.PlayMusic(fireAudio.Id);
            }
        }
        
        private void ReloadBelt()
        {
            // TODO: reload :)
        }
    }
}

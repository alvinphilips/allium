using Fusion;
using UnityEngine;

namespace Game.Scripts
{
    public class Projectile : NetworkBehaviour
    {
        [Networked] public float Damage { get; set; }
        [Networked] public float Range { get; set; }

        [Networked] public float Velocity { get; set; }

        [Networked] private Vector3 StartPos { set; get; }

        public override void Spawned()
        {
            if (Object.HasStateAuthority)
            {
                StartPos = transform.position;
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (!Object.HasStateAuthority) return;

            transform.position += transform.forward * Velocity * Time.deltaTime;

            var distanceSqr = Vector3.SqrMagnitude(transform.position - StartPos);

            if (distanceSqr > Range * Range)
            {
                DestroyProjectile();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!Object.HasStateAuthority) return;

            if(other.TryGetComponent<IDestroyable>(out var destroyable))
            {
                destroyable.Damage(Damage);
            }

            DestroyProjectile();
        }
        
        private void DestroyProjectile()
        {
            Destroy(gameObject);
        }
    }
}

using Fusion;
using UnityEngine;

namespace Game.Scripts
{
    public class Projectile : NetworkBehaviour
    {
        public float Damage { get; set; }
        public float Range { get; set; }

        public float Velocity { get; set; }

        private Vector3 StartPos { set; get; }

        public void Start()
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

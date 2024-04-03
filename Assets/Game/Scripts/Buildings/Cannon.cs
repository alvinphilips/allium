using UnityEngine;

namespace Game.Scripts.Buildings
{
    public class Cannon : DefenceTower
    {
        [SerializeField]
        GameObject projectilePrefab;

        public override void Fire()
        {
            Debug.Log("Firing");

            Projectile p = GameObject.Instantiate(projectilePrefab).GetComponent<Projectile>();
            p.transform.position = projectileFirePos.position;
            p.transform.rotation = projectileFirePos.rotation;
            p.Damage = damage;
            p.Range = range;

        }

        public override Transform GetTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, range, targetLayer);
        
            Debug.Log($"Canon Checking for Targets {colliders.Length}");

            if (colliders.Length > 0)
            {
                foreach (Collider collider in colliders)
                {
                    if (collider.tag == "Enemy") 
                    {

                        target = collider.gameObject.transform;
                        return target;
                    }
                }
            }

            return null;
        }
    }
}

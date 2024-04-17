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
            target = PlacementHandler.Instance.GetClosestTarget(transform.position, ObjectType.Tank, range).transform; 

            return target;
        }
    }
}

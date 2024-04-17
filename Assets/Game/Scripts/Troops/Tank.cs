using UnityEngine;

namespace Game.Scripts.Troops
{
    public class Tank : Troop
    {
        [SerializeField]
        public Transform projectileFirePos;

        [SerializeField]
        public Transform turretTransform;

        [SerializeField]
        GameObject projectilePrefab;

        public void RotateTurret(Quaternion rotation)
        {
            turretTransform.rotation = rotation;
        }

        public override Transform GetTarget()
        {
            Target = PlacementHandler.Instance.GetClosestTarget(transform.position, ObjectType.DefenceBldg, Range).transform;

            return Target;
        }

        public override void Aim(Quaternion rotation)
        {
            RotateTurret(rotation);
        }

        public override void Fire()
        {
            Debug.Log("Firing");

            Projectile p = GameObject.Instantiate(projectilePrefab).GetComponent<Projectile>();
            p.transform.position = projectileFirePos.position;
            p.transform.rotation = projectileFirePos.rotation;
            p.Damage = Damage;
            p.Range = Range;
        }
    }
}

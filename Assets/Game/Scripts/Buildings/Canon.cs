using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : DefenceTower
{
    [SerializeField]
    GameObject projectilePrefab;

    public override void Fire()
    {
        Debug.Log("Firing");

        Projectile p = GameObject.Instantiate(projectilePrefab).GetComponent<Projectile>();
        p.transform.position = projectileFirePos.position;
        p.transform.rotation = projectileFirePos.rotation;
        p.SetProjectile(damage, range);

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

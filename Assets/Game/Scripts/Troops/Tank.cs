using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tank : Troop
{
    

    [SerializeField]
    public Transform projectileFirePos;

    [SerializeField]
    public Transform turretTransform;

    [SerializeField]
    public float turretRotateSpeed = 10f;

    public float fireDelay = 2f;


    //Angle within which turret can fire
    public float fireThreshould;

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

    public void RotateTurret(Quaternion rotation)
    {
        turretTransform.rotation = rotation;
    }

    public override Transform GetTarget()
    {
        target = PlacementHandler.Instance.GetClosestTarget(transform.position, ObjectType.DefenceBldg, range).transform;

        return target;
    }
}

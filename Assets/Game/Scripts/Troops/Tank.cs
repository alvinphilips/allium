using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tank : Troop
{
    [SerializeField]
    public int range;

    [SerializeField]
    public int damage;

    [SerializeField]
    public Transform projectileFirePos;

    [SerializeField]
    public Transform turretTransform;

    [SerializeField]
    public float turretRotateSpeed = 10f;

    public float fireDelay = 2f;

    [SerializeField]
    public LayerMask targetLayer;

    //Angle within which turret can fire
    public float fireThreshould;

    public virtual void Fire()
    {

    }

    public void RotateTurret(Quaternion rotation)
    {
        turretTransform.rotation = rotation;
    }

    public override Transform GetTarget()
    {

        return target;
    }
}

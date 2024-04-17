using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for all defence structures, can have gun / tank with different target types and fire logic 
public class DefenceTower : Building
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

    public Transform target;

    //Angle within which turret can fire
    public float fireThreshould;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        buildingType = BuildingTypes.Defence;
    }

    public virtual void Fire()
    {

    }

    public void RotateTurret(Quaternion rotation)
    {
        turretTransform.rotation = rotation;  
    }

    public virtual Transform GetTarget()
    {
        return target;
    }
}

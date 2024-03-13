using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for all defence structures, can have gun / tank with different target types and fire logic 
public class DefenceTower : Buildings
{
    [SerializeField]
    protected int range;
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected float rotSpeed;
    [SerializeField]
    protected Transform turretTop;

    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        buildingType = BuildingTypes.Defence;
    }

    public virtual void Fire()
    {

    }

    public virtual Transform GetTarget()
    {
        return target;
    }
}

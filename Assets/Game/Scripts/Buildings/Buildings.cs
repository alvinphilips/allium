using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum BuildingTypes
{
    ResourceGeneration,
    Defence,
    UnitSpawn
}

public class Buildings : MonoBehaviour, IDestroyable
{
    protected BuildingTypes buildingType = BuildingTypes.ResourceGeneration;

    protected int level = 1;

    public float Health { get; set; }
    public float DamageMultipyer { get; set; }
    public GameObject owner { get; set; }
    UnityEvent<GameObject> IDestroyable.OnObjectDestroyed { get; set; }

    public void Start()
    {
        Health = 50;
        DamageMultipyer = 1;
        owner = gameObject;
    }

    void IDestroyable.Damage(float damage)
    {
        Health -= damage * DamageMultipyer;
        if (Health <= 0)
            ((IDestroyable)this).DestroyObject();
    }

    void IDestroyable.DestroyObject()
    {
        ((IDestroyable)this).OnObjectDestroyed?.Invoke(owner);
        GameObject.Destroy(owner);
    }
}

using Fusion;
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

public class Building : NetworkBehaviour, IDestroyable
{
    protected BuildingTypes buildingType = BuildingTypes.ResourceGeneration;

    protected int level = 1;

    public float Health { get; set; }
    public float DamageMultiplier { get; set; }
    public GameObject Owner { get; set; }
    public UnityEvent<GameObject> OnObjectDestroyed { get; set; }

    protected virtual void Start()
    {
        Health = 50;
        DamageMultiplier = 1;
        Owner = gameObject;
    }

    public void Damage(float damage)
    {
        Health -= damage * DamageMultiplier;
        
        if (Health <= 0)
        {
            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        OnObjectDestroyed?.Invoke(Owner);
        GameObject.Destroy(Owner);
    }
}

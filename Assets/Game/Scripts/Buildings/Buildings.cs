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
    public UnityEvent<GameObject> OnObjectDestroyed { get; set; }

    public void Damage(float damage)
    {
        Health -= damage * DamageMultipyer;
        if (Health <= 0)
            DestroyObject();
    }

    public void DestroyObject()
    {
        OnObjectDestroyed?.Invoke(owner);
        GameObject.Destroy(owner);
    }
}

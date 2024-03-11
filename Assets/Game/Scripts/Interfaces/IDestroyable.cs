using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDestroyable
{
    public float Health { get; set; }
    public float DamageMultipyer { get; set; }
    public GameObject owner { get; set; }

    public UnityEvent<GameObject> OnObjectDestroyed { get; set; }

    public virtual void Damage(float damage)
    {
        Health -= damage * DamageMultipyer;
        if (Health <= 0)
            DestroyObject();
    }

    public virtual void DestroyObject()
    {
        OnObjectDestroyed?.Invoke(owner);
        GameObject.Destroy(owner);
    }

}

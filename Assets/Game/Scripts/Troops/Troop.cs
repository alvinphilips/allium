using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Troop : MonoBehaviour, IDestroyable
{

    public Transform target;

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

    public virtual Transform GetTarget()
    {
        return target;
    }
}

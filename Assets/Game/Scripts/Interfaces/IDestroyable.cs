using UnityEngine.Events;
using UnityEngine;

public interface IDestroyable
{
    public float Health { get; set; }
    public float DamageMultiplier { get; set; }
    public GameObject owner { get; set; }

    public bool IsDead => Health <= 0;

    public UnityEvent<GameObject> OnObjectDestroyed { get; set; }

    public virtual void Damage(float damage) { }

    public virtual void DestroyObject() { }

}

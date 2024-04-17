using UnityEngine.Events;
using UnityEngine;

public interface IDestroyable
{
    public float Health { get; set; }
    public float DamageMultiplier { get; set; }
    public GameObject Owner { get; set; }

    public bool IsDead => Health <= 0;

    public UnityEvent<GameObject> OnObjectDestroyed { get; set; }

    public void Damage(float damage) { }

    public void DestroyObject() { }

}

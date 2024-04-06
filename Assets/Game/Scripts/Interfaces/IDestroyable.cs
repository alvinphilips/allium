using UnityEngine.Events;
using UnityEngine;

public interface IDestroyable
{

    public bool IsDead => Health <= 0;

    public UnityEvent<GameObject> OnObjectDestroyed { get; set; }

    public virtual void Damage(float damage) { }

    public virtual void DestroyObject() { }

}

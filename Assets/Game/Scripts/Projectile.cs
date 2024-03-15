using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Projectile that can be used by multiple things
public class Projectile : MonoBehaviour
{
    float damage = 0f;
    float range = 0f;

    public float projectileSpeed = 5f;

    Vector3 startPos;

    public void SetProjectile(float damage, float range)
    {
        this.damage = damage;
        this.range = range;

        startPos = transform.position;
    }

    private void Update()
    {
        //transform.Translate;

        transform.position += transform.forward * projectileSpeed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, startPos);

        if (distance > range)
        {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IDestroyable destroyable;

        if(other.TryGetComponent<IDestroyable>(out destroyable))
        {
            destroyable.Damage(damage);
        }

        DestroyProjectile();
    }

    void DestroyProjectile()
    {
        //PlayFx
        //Destroy Object or make it poolable
        Destroy(gameObject);
    }
}

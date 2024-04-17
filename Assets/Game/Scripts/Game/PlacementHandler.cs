using Game.Scripts.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    ResourceBldg,
    DefenceBldg,
    Unit,
    Tank
}

public class PlacementHandler : Singleton<PlacementHandler>
{
    [SerializeField] List<GameObject> resourceBldgs = new List<GameObject>();
    [SerializeField] List<GameObject> defenceBldgs = new List<GameObject>();
    [SerializeField] List<GameObject> troops = new List<GameObject>();
    [SerializeField] List<GameObject> tanks = new List<GameObject>();

    public void AddObject(GameObject obj, ObjectType objectType)
    {
        switch (objectType)
        {
            case ObjectType.ResourceBldg:
                resourceBldgs.Add(obj);
                break;
            case ObjectType.DefenceBldg:
                defenceBldgs.Add(obj);
                break;
            case ObjectType.Unit:
                troops.Add(obj);
                break;
            case ObjectType.Tank:
                tanks.Add(obj);
                break;
        }
    }

    public GameObject GetClosestTarget(Vector3 SrcPos, ObjectType objectType, float range = 1000f)
    {
        List<GameObject> possibleTargets = new List<GameObject>();

        switch(objectType)
        {
            case ObjectType.ResourceBldg:
                possibleTargets = resourceBldgs;
                break;
            case ObjectType.DefenceBldg:
                possibleTargets = defenceBldgs;
                break;
            case ObjectType.Unit:
                possibleTargets = troops;
                break;
            case ObjectType.Tank:
                possibleTargets = tanks;
                break;
        }

        GameObject closestTarget = null;
        float closestDist = 1000f;
        foreach (GameObject target in possibleTargets)
        {
            float dist = Vector3.Distance(SrcPos, target.transform.position);

            if(dist < range)
            {
                if(closestDist > dist)
                {
                    closestTarget = target;
                    closestDist = dist;
                }
            }
        }

        switch (objectType)
        {
            case ObjectType.ResourceBldg:
                resourceBldgs.Remove(closestTarget);
                break;
            case ObjectType.DefenceBldg:
                defenceBldgs.Remove(closestTarget);
                break;
            case ObjectType.Unit:
                troops.Remove(closestTarget);
                break;
            case ObjectType.Tank:
                tanks.Remove(closestTarget);
                break;
        }

        return closestTarget;
    }
}

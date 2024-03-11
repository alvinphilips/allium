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
}

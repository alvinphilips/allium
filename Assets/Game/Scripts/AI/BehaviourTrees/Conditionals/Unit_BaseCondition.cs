using BehaviorDesigner.Runtime.Tasks;
using Game.Scripts.Troops;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_BaseCondition : Conditional
{
    protected Unit unit;

    public override void OnStart()
    {
        base.OnStart();
        unit = GetComponent<Unit>();
    }

}

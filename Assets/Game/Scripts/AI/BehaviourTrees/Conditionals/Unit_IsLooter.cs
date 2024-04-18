using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_IsLooter : Unit_BaseCondition
{
    public override TaskStatus OnUpdate()
    {
        return (unit.unitType == Game.Scripts.Troops.UnitType.Looters ? TaskStatus.Success : TaskStatus.Failure);       
    }
}

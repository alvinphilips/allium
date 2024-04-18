using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_IsDamaged : Unit_BaseCondition
{
    public override TaskStatus OnUpdate()
    {
        return (unit.Health < 50 ? TaskStatus.Success : TaskStatus.Failure);
    }
}

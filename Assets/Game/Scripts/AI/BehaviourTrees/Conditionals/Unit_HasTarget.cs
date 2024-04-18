using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_HasTarget : Unit_BaseCondition
{
    public override TaskStatus OnUpdate()
    {
        return (unit.Target == null ? TaskStatus.Failure : TaskStatus.Success);
    }
}
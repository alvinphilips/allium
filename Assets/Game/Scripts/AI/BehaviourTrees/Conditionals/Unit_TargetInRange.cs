using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_TargetInRange : Unit_BaseCondition
{
    public override TaskStatus OnUpdate()
    {
        if(unit.Target != null)
        {
            if (Vector3.Distance(unit.transform.position, unit.Target.transform.position) < unit.Range)
                return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}

using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_SeekTarget : Unit_BaseAction
{
    public override TaskStatus OnUpdate()
    {
        base.OnUpdate();

        if(unit.GetTarget() == null)
            return TaskStatus.Running;

        return TaskStatus.Success;
    }

}

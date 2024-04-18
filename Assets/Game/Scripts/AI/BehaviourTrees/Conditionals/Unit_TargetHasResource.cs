using BehaviorDesigner.Runtime.Tasks;
using Game.Scripts.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_TargetHasResource : Unit_BaseCondition
{
    public override TaskStatus OnUpdate()
    {
        if(unit.Target != null)
        {
            ResourceGenerator generator = unit.Target.GetComponent<ResourceGenerator>();
            if(generator != null) 
            { 
                return TaskStatus.Success;
            }
        }

        return TaskStatus.Failure;
    }
}

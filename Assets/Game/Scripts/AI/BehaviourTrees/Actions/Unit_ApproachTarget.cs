using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_ApproachTarget : Unit_BaseAction
{
    SteeringAgent steeringAgent;

    public override void OnStart()
    {
        base.OnStart();

        steeringAgent = unit.gameObject.GetComponent<SteeringAgent>();

        Debug.Assert(steeringAgent != null, $"{unit.gameObject.name} requires Steering Agent Component!");

        steeringAgent.SetTarget(unit.Target.position);
    }

    public override TaskStatus OnUpdate()
    {
        base.OnUpdate();

        if ( steeringAgent != null ) 
        {
            if (steeringAgent.bReachedGoal && steeringAgent.target == Vector3.zero)
            {
                return TaskStatus.Success;
            }
            else 
            {
                return TaskStatus.Running;
            }
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}

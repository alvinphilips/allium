using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Game.Scripts.Buildings;
using Game.Scripts.Troops;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Steal : Unit_BaseAction
{
    [SerializeField]
    SharedInt genFrequency;
    
    private float elapsedTime;

    ResourceGenerator resourceGenerator;

    public override void OnStart()
    {
        resourceGenerator = unit.Target.GetComponent<ResourceGenerator>(); 
    }

    public override TaskStatus OnUpdate()
    {
        if(resourceGenerator == null)
        {
            Debug.Log("Resource Generator null in Target");
            return TaskStatus.Failure;
        }

        elapsedTime += Time.deltaTime;

        if (elapsedTime > genFrequency.Value)
        {
            bool looting = resourceGenerator.LootResource();
            elapsedTime = 0;

            if (!looting)
            {
                return TaskStatus.Success;
            }
        }

        return TaskStatus.Running;
    }
}

using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Unit_Attack : Unit_BaseAction
{
    public override TaskStatus OnUpdate()
    {
        base.OnUpdate();

        if (unit.target != null)
        {
            Vector3 vectorToTarget = (unit.target.position - owner.transform.position);
            Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget);
            Quaternion rotation = Quaternion.Lerp(unit.transform.rotation, targetRotation, 50f * Time.deltaTime);
            unit.Aim(rotation);

            float angle = Vector3.Angle(tank.turretTransform.forward, (tank.target.position - owner.transform.position));

            if (Mathf.Abs(angle) < unit.fireThreshould)
            {
                if (currentTime > nextFireTime)
                {
                    tank.Fire();
                    nextFireTime = currentTime + tank.fireDelay;
                }
            }

            currentTime += Time.deltaTime;

            float distanceToTarget = vectorToTarget.magnitude;

            if (distanceToTarget > tank.range)
            {
                fsm.ChangeState(T_ApproachTarget);

            }
        }
    }
}

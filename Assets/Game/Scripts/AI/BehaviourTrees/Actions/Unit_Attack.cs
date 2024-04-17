using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Unit_Attack : Unit_BaseAction
{
    SharedFloat currentTime = 0f;
    SharedFloat nextFireTime = 0;

    public override TaskStatus OnUpdate()
    {
        base.OnUpdate();

        if (unit.target != null)
        {
            Vector3 vectorToTarget = (unit.target.position - unit.transform.position);
            Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget);
            Quaternion rotation = Quaternion.Lerp(unit.transform.rotation, targetRotation, 50f * Time.deltaTime);
            unit.Aim(rotation);

            float angle = Vector3.Angle(unit.transform.forward, vectorToTarget);

            if (Mathf.Abs(angle) < unit.fireThreshold)
            {
                if (currentTime.Value > nextFireTime.Value)
                {
                    unit.Fire();
                    nextFireTime.Value = currentTime.Value + unit.fireDelay;
                }
            }

            currentTime.Value += Time.deltaTime;

            float distanceToTarget = vectorToTarget.magnitude;

            if (distanceToTarget > unit.range)
            {
                return TaskStatus.Success;
            }
            
            return TaskStatus.Running;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}

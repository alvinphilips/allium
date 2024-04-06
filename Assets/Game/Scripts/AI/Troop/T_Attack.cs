using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Attack : T_BaseState
{
    float currentTime = 0f;
    float nextFireTime = 0;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Rotate to target and keep firing
        if (tank.target != null)
        {
            Vector3 vectorToTarget = (tank.target.position - owner.transform.position);
            Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget);
            Quaternion rotation = Quaternion.Lerp(tank.turretTransform.rotation, targetRotation, tank.turretRotateSpeed * Time.deltaTime);

            tank.RotateTurret(rotation);

            float angle = Vector3.Angle(tank.turretTransform.forward, (tank.target.position - owner.transform.position));

            if (Mathf.Abs(angle) < tank.fireThreshould)
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
        else
        {
            fsm.ChangeState(T_SeekTarget);
        }

    }


}

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
        if (troop.target != null)
        {
            Vector3 vectorToTarget = (troop.target.position - owner.transform.position);
            Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget);
            Quaternion rotation = Quaternion.Lerp(troop.turretTransform.rotation, targetRotation, defenceTower.turretRotateSpeed * Time.deltaTime);

            .RotateTurret(rotation);

            float angle = Vector3.Angle(troop.turretTransform.forward, (troop.target.position - owner.transform.position));

            if (Mathf.Abs(angle) < troop.fireThreshould)
            {
                if (currentTime > nextFireTime)
                {
                    troop.Fire();
                    nextFireTime = currentTime + troop.fireDelay;
                }
            }

            currentTime += Time.deltaTime;

            float distanceToTarget = vectorToTarget.magnitude;

            if (distanceToTarget > troop.range)
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

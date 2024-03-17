using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFT_Attack : DFT_BaseState
{
    float currentTime = 0f;
    float nextFireTime = 0;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Rotate to target and keep firing
        if(defenceTower.target != null)
        {
            Vector3 vectorToTarget = (defenceTower.target.position - owner.transform.position);
            Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget);
            Quaternion rotation = Quaternion.Lerp(defenceTower.turretTransform.rotation, targetRotation, defenceTower.turretRotateSpeed * Time.deltaTime);

            defenceTower.RotateTurret(rotation);

            float angle = Vector3.Angle(defenceTower.turretTransform.forward, (defenceTower.target.position - owner.transform.position));

            if (Mathf.Abs(angle) < defenceTower.fireThreshould)
            {
                if(currentTime > nextFireTime)
                {
                    defenceTower.Fire();
                    nextFireTime = currentTime + defenceTower.fireDelay;
                }
            }

            currentTime += Time.deltaTime;
            
            float distanceToTarget = vectorToTarget.magnitude;

            if(distanceToTarget > defenceTower.range)
            {
                defenceTower.target = null;
                
            }
        }
        else
        {
            fsm.ChangeState(DFT_SeekTarget);
        }

    }


}

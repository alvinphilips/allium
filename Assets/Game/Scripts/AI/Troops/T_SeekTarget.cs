using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_SeekTarget : T_BaseState
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        tank.Target = tank.GetTarget();

        if (tank.Target != null)
        {
            fsm.ChangeState(T_ApproachTarget);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFT_SeekTarget : DFT_BaseState
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(defenceTower.GetTarget() != null)
        {
            fsm.ChangeState(DFT_Attack);
        }
    }
}

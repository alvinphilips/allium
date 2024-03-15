using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFT_BaseState : FSMBaseState<DefenceTowerFSM>
{
    protected DefenceTower defenceTower;

    static protected readonly int DFT_SeekTarget = Animator.StringToHash("SeekTarget");
    static protected readonly int DFT_Attack = Animator.StringToHash("Attack");

    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);

        defenceTower = owner.GetComponent<DefenceTower>();
    }
}

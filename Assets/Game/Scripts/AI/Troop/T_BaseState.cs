using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_BaseState : FSMBaseState<TankFSM>
{
    static protected readonly int T_SeekTarget = Animator.StringToHash("SeekTarget");
    static protected readonly int T_Attack = Animator.StringToHash("Attack");
    static protected readonly int T_ApproachTarget = Animator.StringToHash("Approach");
    static protected readonly int T_Flee = Animator.StringToHash("Flee");

    protected Tank tank;
}

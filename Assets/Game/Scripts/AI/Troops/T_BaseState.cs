using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Troops;
using UnityEngine;

public class T_BaseState : FSMBaseState<TankFSM>
{
    static protected readonly int T_SeekTarget = Animator.StringToHash("SeekTarget");
    static protected readonly int T_Attack = Animator.StringToHash("Attack");
    static protected readonly int T_ApproachTarget = Animator.StringToHash("ApproachTarget");
    static protected readonly int T_Flee = Animator.StringToHash("Flee");

    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);
        tank = owner.GetComponent<Tank>();
    }

    protected Tank tank;
}

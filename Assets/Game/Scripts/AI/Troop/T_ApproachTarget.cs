using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_ApproachTarget : T_BaseState
{
    SteeringAgent steeringAgent;

    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);

        steeringAgent = _owner.GetComponent<SteeringAgent>();
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        steeringAgent.SetTarget(tank.target.position);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (steeringAgent.bReachedGoal && steeringAgent.target == Vector3.zero)
        {
            fsm.ChangeState(T_Attack);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        steeringAgent.ResetTarget();
    }
}

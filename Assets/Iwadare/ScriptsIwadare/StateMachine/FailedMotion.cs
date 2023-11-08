using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailedMotion : IState
{
    public void InitialState()
    {
        Debug.Log("���s�G���[�V�����I���������I");
    }

    public void OnEnterState(StateMachineController stateMachine)
    {
        stateMachine.Anim.Play(stateMachine.SuccessName);
        Debug.Log(stateMachine._nGWordbool ?
            "�Ղ�j����I����؂��Ă邱�̓��͉��H�͂��A�Ȃ�ق�" :
            "�����Ղ�Ղ�ہI�I");
    }

    public void OnUpdate(StateMachineController stateMachine)
    {
        Debug.Log(stateMachine._nGWordbool ? 
            "�Ղ�Ղ�ۂ��Č����̂ˁA���ˎ�ɁA�J�`���R�`���Ȑn�ŉ��ł��؂ꂻ���I" : 
            "�Ղ�Ղ�I�Ղ�Ձ[�[�[��I");
    }

    public void OnExitState(StateMachineController stateMachine)
    {
        Debug.Log(stateMachine._nGWordbool ? 
            "�`���Q���؂�؂��āA���̓��̐؂ꖡ�A�������Ȃ��̂������Ă݂�ˁI" : 
            "�܂�{���̂��a���Ă��܂���...");
    }

}
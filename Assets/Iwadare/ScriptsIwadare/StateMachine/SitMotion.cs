using StateMachine;
using System;
using UnityEngine;

[Serializable]
public class SitMotion : IState
{
    public void InitialState(bool disDebugLog)
    {
        DebugLogUtility.PrankLog("座る！準備完了！", disDebugLog);
    }

    public void OnEnterState(StateMachineController stateMachine)
    {
        stateMachine.Anim.Play(stateMachine.SitName);
        if (stateMachine._sitScripts != null && stateMachine._avatorTrams != null)
        {
            stateMachine._avatorTrams.position = stateMachine._sitScripts.SitDownPosition();
            stateMachine._avatorTrams.rotation = Quaternion.Euler(stateMachine._sitScripts.SitDownRotation());
        }
        DebugLogUtility.PrankLog("座るかあ。", stateMachine.DisplayLog);
    }

    public void OnUpdate(StateMachineController stateMachine)
    {
        return;
    }

    public void OnExitState(StateMachineController stateMachine)
    {
        stateMachine.Anim.Play(stateMachine.SitName + "End");
        if (stateMachine._sitScripts != null && stateMachine._avatorTrams != null)
        {
            stateMachine._avatorTrams.position = stateMachine._sitScripts.StandUp();
        }
        DebugLogUtility.PrankLog("立つかあ", stateMachine.DisplayLog);
    }
}

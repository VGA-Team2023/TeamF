using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitsMotion : IState
{
    public void InitialState(bool disDebugLog)
    {
        DebugLogUtility.PrankLog("HitsA๕ฎนI",disDebugLog);
    }

    public void OnEnterState(StateMachineController stateMachine)
    {
        DebugLogUtility.PrankLog("ษฤฅI",stateMachine.DisplayLog);
        if (stateMachine._sitScripts != null && stateMachine._avatorTrams != null)
        {
            stateMachine._avatorTrams.position = stateMachine._sitScripts.SitDownPosition();
            stateMachine._avatorTrams.rotation = Quaternion.Euler(stateMachine._sitScripts.SitDownRotation());
        }
        stateMachine.Anim.Play(stateMachine.HitsAniName);
    }

    public void OnUpdate(StateMachineController stateMachine)
    {

    }

    public void OnExitState(StateMachineController stateMachine)
    {
        if (stateMachine._sitScripts != null && stateMachine._avatorTrams != null)
        {
            stateMachine._avatorTrams.position = stateMachine._sitScripts.StandUp();
        }
        DebugLogUtility.PrankLog("A้ํB",stateMachine.DisplayLog);
    }

    
}

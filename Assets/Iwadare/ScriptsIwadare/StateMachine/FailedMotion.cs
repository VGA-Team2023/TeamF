using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailedMotion : IState
{
    public void InitialState(bool disDebugLog)
    {
        DebugLogUtility.PrankLog("失敗エモーション！準備完了！", disDebugLog);
    }

    public void OnEnterState(StateMachineController stateMachine)
    {
        stateMachine.Anim.Play(stateMachine.FailedName);
        DebugLogUtility.PrankLog(stateMachine.NGWordbool ?
            "ぷん男くん！肉を切ってるこの刀は何？はあ、なるほど" :
            "名刀ぷんぷん丸！！", stateMachine.DisplayLog);
    }

    public void OnUpdate(StateMachineController stateMachine)
    {
        DebugLogUtility.PrankLog(stateMachine.NGWordbool ? 
            "ぷんぷん丸って言うのね、取るね手に、カチンコチンな刃で何でも切れそう！" : 
            "ぷんぷん！ぷんぷーーーん！", stateMachine.DisplayLog);
    }

    public void OnExitState(StateMachineController stateMachine)
    {
        DebugLogUtility.PrankLog(stateMachine.NGWordbool ? 
            "チンゲン菜を切って、この刀の切れ味、いかがなものか試してみるね！" : 
            "つまら怒ものを斬ってしまった...", stateMachine.DisplayLog);
    }

}

using StateMachine;

public class IdleMotion : IState
{
    public void InitialState(bool disDebugLog)
    {
        DebugLogUtility.PrankLog("�x�~�I���������I", disDebugLog);
    }

    public void OnEnterState(StateMachineController stateMachine)
    {
        DebugLogUtility.PrankLog("�~�܂�", stateMachine.DisplayLog);
        stateMachine.Anim.Play(stateMachine.IdleAniName);
    }

    public void OnUpdate(StateMachineController stateMachine)
    {
       return;
    }

    public void OnExitState(StateMachineController stateMachine)
    {
        DebugLogUtility.PrankLog("�~�܂�����", stateMachine.DisplayLog);
    }
}

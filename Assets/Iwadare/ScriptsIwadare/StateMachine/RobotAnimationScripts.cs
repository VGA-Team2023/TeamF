using StateMachine;
using UnityEngine;
using UnityEngine.UI;
using Alpha;

public class RobotAnimationScripts : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private StateMachineController _stateMachine;

    [SerializeField]
    private Button _changeSitChairButton;

    [SerializeField]
    private SitScripts[] _allSitScripts;

    [SerializeField]
    bool _animationCallBackTestBool;

    private SitRequest _sitRequest;

    private int _chairCount = 0;

    void Start()
    {
        _sitRequest = FindObjectOfType<SitRequest>();
        _allSitScripts = _sitRequest.SitScriptsRequest();
        if (_changeSitChairButton != null) { _changeSitChairButton.onClick.AddListener(() => SitReceipt(_allSitScripts[(_chairCount + 1) % _allSitScripts.Length])); }
        _stateMachine.Init(ref _animator);
    }

    private void OnEnable()
    {
        if (_animationCallBackTestBool)
        {
            AnimationCallBackTest.OnAnimationWalk += WalkAnimation;
            AnimationCallBackTest.OnAnimationSit += SitAnimation;
            AnimationCallBackTest.OnAnimationSuccess += SuccessAnimation;
            AnimationCallBackTest.OnAnimationFailed += FailedAnimation;
            AnimationCallBackTest.OnAnimationStay += WaitState;
        }
        FerverTime.OnEnter += DanceAnimation;
        if (FerverTime.IsFerver) {  DanceAnimation(); };
    }

    private void OnDisable()
    {
        if (_animationCallBackTestBool)
        {
            AnimationCallBackTest.OnAnimationWalk -= WalkAnimation;
            AnimationCallBackTest.OnAnimationSit -= SitAnimation;
            AnimationCallBackTest.OnAnimationSuccess -= SuccessAnimation;
            AnimationCallBackTest.OnAnimationFailed -= FailedAnimation;
            AnimationCallBackTest.OnAnimationStay -= WaitState;
        }
        FerverTime.OnEnter -= DanceAnimation;
    }

    void Update()
    {
        _stateMachine.Update();
    }

    /// <summary>歩きアニメーション</summary>
    public void WalkAnimation()
    {
        _stateMachine.OnChangeState(_stateMachine.GetWalk);
    }

    /// <summary>座るアニメーション</summary>
    public void SitAnimation()
    {
        _stateMachine.OnChangeState(_stateMachine.GetSit);
    }

    /// <summary>成功アニメーション</summary>
    public void SuccessAnimation()
    {
        _stateMachine.OnChangeState(_stateMachine.GetSuccessMotion);
    }

    /// <summary>失敗アニメーション</summary>
    public void FailedAnimation()
    {
        _stateMachine.OnChangeState(_stateMachine.GetFailedMotion);
    }

    /// <summary>ダンスアニメーション</summary>
    public void DanceAnimation()
    {
        _stateMachine.FalseFeverTimeBool();
        _stateMachine.OnChangeState(_stateMachine.GetDance);
        _stateMachine.FeverTimeBool();
    }

    /// <summary>成功モーションと歩きモーションのAnimationEnd表示用</summary>
    public void WaitState()
    {
        if (_stateMachine.CurrentState != _stateMachine.GetSit)
        {
            _stateMachine.OnChangeState(_stateMachine.GetWaitState);
        }
    }

    /// <summary>座る場所指定(引数SitScripts)</summary>
    /// <param name="sitScripts">SitScripts</param>
    public void SitReceipt(SitScripts sitScripts)
    {
        _stateMachine._sitScripts = sitScripts;
        _chairCount = (_chairCount + 1) % _allSitScripts.Length;
        Debug.Log(_chairCount);
    }

    /// <summary>座る場所指定(引数int)</summary>
    public void SitReceipt(int index)
    {
        _stateMachine._sitScripts = _allSitScripts[index];
        Debug.Log(index);
    }
}

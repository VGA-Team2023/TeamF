using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Alpha
{
    public struct FireMessage { }

    /// <summary>
    /// 一定時間経過後に射撃を行うステート
    /// アニメーション再生のステートのEnterに射撃の非同期処理を追加する
    /// </summary>
    public class FireState : AnimationState
    {
        [Header("射撃の設定")]
        [SerializeField] float _fireDelay = 0.5f;

        CancellationTokenSource _cts = new();

        public override StateType Type => StateType.Fire;

        void OnDestroy()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
            }
        }

        protected override void Enter()
        {
            // アニメーションの再生処理
            base.Enter();

            // 遅延して射撃
            _cts = new();
            FireAsync(_cts.Token).Forget();
        }

        protected override void Exit()
        {
            // 何もしていないが一応
            base.Exit();

            _cts.Cancel();
        }

        /// <summary>
        /// 一定時間後に射撃したメッセージを送信するする
        /// </summary>
        async UniTaskVoid FireAsync(CancellationToken token)
        {
            if (token.IsCancellationRequested) return;
            Cri.PlaySE("SE_Robber_GunPrepare");

            await UniTask.Delay(System.TimeSpan.FromSeconds(0.5f), cancellationToken: token);
            Cri.PlaySE3D(transform.position, "SE_Robber_Voice_1");

            await UniTask.Delay(System.TimeSpan.FromSeconds(_fireDelay), cancellationToken: token);
            Cri.PlaySE("SE_Robber_GunShot");

            OrderScoreSender.SendScore(OrderResult.Failure, ActorType.Muscle, Tension,
                transform.position, ScoreKey.Robber);

            MessageBroker.Default.Publish(new FireMessage());
            CameraShakeMessageSender.SendMessage(3.0f); // ベタ書き

            for (int i = 0; i < 5; i++)
            {
                await UniTask.Delay(System.TimeSpan.FromSeconds(0.2f), cancellationToken: token);
                Cri.PlaySE("SE_Robber_GunShot");
            }
        }
    }
}
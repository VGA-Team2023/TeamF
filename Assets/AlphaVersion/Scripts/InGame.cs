using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Alpha
{
    public struct GameStartMessage { }
    public struct GameOverMessage { }

    /// <summary>
    /// 各機能を用いてインゲームの制御を行うクラス
    /// </summary>
    public class InGame : MonoBehaviour
    {
        [SerializeField] InGameSettingsSO _settings;
        [SerializeField] GameStartEvent _gameStartEvent;
        [SerializeField] GameOverEvent _gameOverEvent;
        [SerializeField] TimerUI _timerUI;
        [SerializeField] ActorSpawnTimer _actorSpawnTimer;

        /// <summary>
        /// 非同期処理の実行
        /// </summary>
        void Start()
        {
            CancellationTokenSource cts = new();
            UpdateAsync(cts.Token).Forget();

            // オブジェクトの破棄時にトークンをDisposeする
            this.OnDestroyAsObservable().Subscribe(_ => { cts.Cancel(); cts.Dispose(); });
        }

        /// <summary>
        /// インゲームの流れ
        /// </summary>
        async UniTaskVoid UpdateAsync(CancellationToken token)
        {
            // ゲーム開始の演出
            await _gameStartEvent.PlayAsync(token);
            
            SendGameStartMessage();

            // 時間切れまでループ
            float elapsed = 0;
            while (elapsed <= _settings.TimeLimit)
            {
                // 経過時間をUIに表示
                _timerUI.Draw(_settings.TimeLimit, elapsed);

                // キャラクター生成用のタイマーを進める
                _actorSpawnTimer.Tick();

                elapsed += Time.deltaTime;
                await UniTask.Yield(token);
            }

            SendGameOverMessage();

            // ゲーム終了の演出
            await _gameOverEvent.PlayAsync("成績", token);
        }

        /// <summary>
        /// ゲーム開始のメッセージング
        /// </summary>
        void SendGameStartMessage()
        {
            MessageBroker.Default.Publish(new GameStartMessage());
        }

        /// <summary>
        /// ゲームオーバーのメッセージング
        /// </summary>
        void SendGameOverMessage()
        {
            MessageBroker.Default.Publish(new GameOverMessage());
        }
    }
}
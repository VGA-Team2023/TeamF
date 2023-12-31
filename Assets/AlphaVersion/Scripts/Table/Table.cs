using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine.Events;

namespace Alpha
{
    public enum OrderResult
    {
        Success,
        Failure,
        Unsettled, // 未確定
        Defeated,  // 撃破されてラグドールになる
    }

    /// <summary>
    /// 席(客がアイテムをキャッチするエリア)のクラス
    /// </summary>
    public class Table : MonoBehaviour, ITableControl
    {
        [SerializeField] CatchTimer _timer;
        [SerializeField] CatchCollision _collision;
        [SerializeField] CatchTransform _transform;
        [SerializeField] OrderView _view;

        ExtendCTS _cts = new();
        
        void Awake()
        {
            // ゲームオーバー時にトークンをDisposeする
            MessageBroker.Default.Receive<GameOverMessage>()
                .Subscribe(_ => _cts.Dispose()).AddTo(gameObject);
        }

        void OnDestroy()
        {
            _cts.Dispose();
        }

        /// <summary>
        /// キャッチするアイテムと、コールバックを登録して、有効化する
        /// </summary>
        public void Valid(float timeLimit, ItemType order, UnityAction<OrderResult> onCatched = null)
        {
            CatchAsync(timeLimit, order, onCatched).Forget();
        }

        /// <summary>
        /// コールバックを登録削除して無効化する
        /// </summary>
        public void Invalid()
        {
            _cts.Cancel();
            _view.Inactive();
            _timer.Invisible();
        }

        /// <summary>
        /// 時間切れ、キャッチ成功、外部からキャンセル のいずれかまで待つ。
        /// 外部からキャンセルした場合はコールバックが呼ばれないので注意
        /// タイマーの時間切れ: 失敗
        /// コライダーでキャッチ判定: 成功
        /// </summary>
        async UniTaskVoid CatchAsync(float timeLimit, ItemType order, UnityAction<OrderResult> onCatched = null)
        {
            // 無効にせず連続で有効にした場合、キャンセルされないのでチェックしておく
            if (!_cts.IsCancellationRequested) _cts.Cancel();
            _cts = new();

            // 位置とサイズ変更
            Vector3 setPosition = _transform.SetRandomPosition();
            _view.Active(order, setPosition);

            // キャッチ範囲内を掃除、結果を反映するために一応1フレーム待つ
            //CleanTableRange();
            await UniTask.Yield(_cts.Token);

            // 時間切れとキャッチ判定のどちらかが完了するまで待つ
            (int win, OrderResult timerResult, OrderResult collisionResult) result;
            result = await UniTask.WhenAny(
                _timer.WaitAsync(timeLimit, _cts.Token),
                _collision.WaitAsync(order, _cts.Token));

            if (result.win == 0)
            {
                // 時間切れで 失敗
                onCatched?.Invoke(result.timerResult);
            }
            else
            {
                // キャッチ判定で 成功
                onCatched?.Invoke(result.collisionResult);
            }
        }

        /// <summary>
        /// キャッチ範囲のアイテムを削除する
        /// </summary>
        public void CleanTableRange()
        {
            // TODO:NonAllocにする
            Collider[] result = Physics.OverlapSphere(_transform.Position, _transform.Radius);
            for (int i = result.Length - 1; i >= 0; i--)
            {
                if (result[i].TryGetComponent(out ICatchable item))
                {
                    item.Catch();
                }
            }
        }
    }
}
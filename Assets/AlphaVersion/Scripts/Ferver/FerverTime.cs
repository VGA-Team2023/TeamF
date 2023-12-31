using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;

namespace Alpha
{
    /// <summary>
    /// フィーバータイムの開始/終了の切り替え時のメッセージングに使用する構造体
    /// </summary>
    public struct FerverTimeMessage { }

    /// <summary>
    /// フィーバータイムの開始/終了を制御するクラス
    /// </summary>
    public class FerverTime : MonoBehaviour
    {
        /// <summary>
        /// フィーバータイム開始のコールバック
        /// </summary>
        public static event UnityAction OnEnter;
        /// <summary>
        /// フィーバータイム終了のコールバック
        /// </summary>
        public static event UnityAction OnExit;

        static FerverTime _instance;

        [SerializeField] FerverTrigger _trigger;
        [Header("デバッグ用:Fキーでオンオフ切り替え")]
        [SerializeField] bool _isDebug = true;

        bool _isFerver;

        public static bool IsFerver => _instance._isFerver;
        public static bool IsNormal => !_instance._isFerver;

        void Awake()
        {
            _instance ??= this;
        }

        void OnEnable()
        {
            // 発生条件を管理させる
            if (_instance == this) _trigger.OnFerverEnter += Ferver;
        }

        void OnDisable()
        {
            if (_instance == this) _trigger.OnFerverEnter -= Ferver;
        }

        void OnDestroy()
        {
            _instance = null;
            _isFerver = false;
        }

        void Update()
        {
            // デバッグ用にキー入力で切り替えられる
            if (_isDebug && Input.GetKeyDown(KeyCode.F))
            {
                Switch(!_isFerver);
            }
        }

        /// <summary>
        /// フィーバータイム開始、コールバック登録用
        /// </summary>
        void Ferver() => Switch(true);

        /// <summary>
        /// フィーバータイムと通常状態を切り替える
        /// </summary>
        void Switch(bool flag)
        {
            _isFerver = flag;

            // コールバック呼び出し
            if (_isFerver) OnEnter?.Invoke();
            else OnExit?.Invoke();

            // メッセージング
            MessageBroker.Default.Publish(new FerverTimeMessage());
        }
    }
}

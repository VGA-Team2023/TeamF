using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alpha
{
    /// <summary>
    /// 客の生成確率に関する設定
    /// </summary>
    [CreateAssetMenu(fileName = "SpawnRateSettingsSO", menuName = "Settings/SpawnRateSettings")]
    [System.Serializable]
    public class SpawnRateSettingsSO : ScriptableObject
    {
        [Header("<color=#00FF76>客の生成間隔(秒)</color>")]
        [SerializeField] float _customerSpawnRate = 3.0f;

        public float CustomerSpawnRate => _customerSpawnRate;
    }
}

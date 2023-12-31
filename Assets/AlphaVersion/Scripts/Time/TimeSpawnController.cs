using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alpha
{
    /// <summary>
    /// 時間経過による客とギミックの生成を行うクラス
    /// </summary>
    public class TimeSpawnController : MonoBehaviour
    {
        [SerializeField] GimmickSettingsSO _gimmickSettings;
        [SerializeField] SpawnRateSettingsSO _spawnRateSettings;
        [SerializeField] ActorSpawnManager _actorSpawnManager;
        [SerializeField] TumbleweedSpawner _tumbleweedSpawner;
        [SerializeField] UfoSpawner _ufoSpawner;

        float _customerElapsed;
        float _robberElapsed;
        float _tumbleweedElapsed;
        float _ufoElapsed;
        int _robberTimingIndex;
        int _tumbleweedIndex;
        int _ufoIndex;

        /// <summary>
        /// インゲーム側から毎フレーム呼ぶことで時間経過により、一定間隔で生成する
        /// </summary>
        public void Tick(float elapsed)
        {
            _customerElapsed += Time.deltaTime;
            _robberElapsed += Time.deltaTime;
            _tumbleweedElapsed += Time.deltaTime;
            _ufoElapsed += Time.deltaTime;

            // 客
            if (_customerElapsed > _spawnRateSettings.CustomerSpawnRate)
            {
                _customerElapsed = 0;
                _actorSpawnManager.TrySpawnRandomCustomer();
            }

            // 強盗
            if (_robberTimingIndex < _gimmickSettings.Robber.Max &&
                _robberElapsed > _gimmickSettings.Robber.Timing[_robberTimingIndex])
            {
                Cri.PlaySE("SE_Robber_In");
                _actorSpawnManager.SpawnRobber();
                _robberTimingIndex++;
            }

            // タンブル
            if (_tumbleweedIndex < _gimmickSettings.Tumbleweed.Max &&
                _tumbleweedElapsed > _gimmickSettings.Tumbleweed.Timing[_tumbleweedIndex].Elapsed)
            {
                int count = _gimmickSettings.Tumbleweed.Timing[_tumbleweedIndex].Count;
                _tumbleweedSpawner.Spawn(count);
                _tumbleweedIndex++;

            }

            // UFO
            if (_ufoIndex < _gimmickSettings.UFO.Max &&
                _ufoElapsed > _gimmickSettings.UFO.Timing[_ufoIndex])
            {
                _ufoSpawner.Spawn();
                _ufoIndex++;
            }
        }
    }
}
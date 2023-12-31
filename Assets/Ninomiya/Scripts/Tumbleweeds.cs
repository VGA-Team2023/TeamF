using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tumbleweeds : MonoBehaviour
{
    [SerializeField, Header("当たり判定のためのTag")]private string _collisionTag;

    [SerializeField, Header("消えるまでの時間")] private float _timeLimit;

    private float _currntTime = 0f; //現在の時間

    private bool _timeBool = false; //時間計測のため


    private void Update()
    {
        if(_timeBool)
        {
            _currntTime += Time.deltaTime;

            if(_currntTime >= _timeLimit)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        _timeBool = false;
        _currntTime = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == _collisionTag)
        {
            _timeBool = true;
        }
    }
}

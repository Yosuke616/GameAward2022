using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfChildrenChanged : MonoBehaviour
{

    [SerializeField]private int _childCount = 0;
    public int _lowerLimitNum = 15;

    private void Start()
    {
        _childCount = transform.childCount;
    }

    private void Update()
    {
        // 子オブジェクト数に変更があった場合に動作
        if (_childCount != transform.childCount)
        {
            // 子オブジェクトの数を表示する
            Debug.Log("CheckIfChildrenChanged   " + _childCount + "  → " + transform.childCount + "limit:" + _lowerLimitNum);

            // 子オブジェクトの数が一定の数以下になったらこのオブジェクトを消す
            if (transform.childCount < _lowerLimitNum) Destroy(this.gameObject);

            // 子オブジェクトの数を更新
            _childCount = transform.childCount;
        }
    }

}
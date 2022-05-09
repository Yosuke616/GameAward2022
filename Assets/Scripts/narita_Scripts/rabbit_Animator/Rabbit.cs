using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    public Animator rabbitAnimator;
    public GameObject openingCamera;

    private bool onceFunc;

    // Start is called before the first frame update
    void Start()
    {
        onceFunc = true;
    }

    // Update is called once per frame
    void Update()
    {
        // 現在のアニメーション状態を取得
        var state = rabbitAnimator.GetCurrentAnimatorStateInfo(0); // 引数はAnimatorのLayersの番号

        // アニメーション状態が"Move" → "Idle"に遷移した場合
        if(onceFunc && state.IsName("RabbitIdle"))
        {
            onceFunc = false;

            openingCamera.GetComponent<ZoomOut>().ZoomStart();
            // オープニングカメラのズームアウト機能をONにする
            Debug.LogWarning("待機だよ");
        }
    }
}

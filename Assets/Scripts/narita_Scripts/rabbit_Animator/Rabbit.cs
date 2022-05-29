using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    public Animator rabbitAnimator;
    public GameObject openingCamera;

    private bool onceFunc;
	private ModelAnimation rabbitAnimation;

	// Start is called before the first frame update
	void Start()
    {
        onceFunc = true;

		// 子オブジェクトからコンポーネントを取得
		rabbitAnimation = transform.Find("Usagi_Anim_Run").gameObject.GetComponent<ModelAnimation>();
	}

    // Update is called once per frame
    void Update()
    {
        // 現在のアニメーション状態を取得
        var state = rabbitAnimator.GetCurrentAnimatorStateInfo(0); // 引数はAnimatorのLayersの番号

        if(onceFunc && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 1")))
        {
            rabbitAnimator.SetFloat("Speed", 100);
            onceFunc = false;

            //rabbitAnimation.SetAnim("Take 001");

            openingCamera.GetComponent<ZoomOut>().ZoomStart();
        }

        // アニメーション状態が"Move" → "Idle"に遷移した場合
        if(onceFunc && state.IsName("RabbitIdle"))
        {
            onceFunc = false;

			//rabbitAnimation.SetAnim("Take 001");

            openingCamera.GetComponent<ZoomOut>().ZoomStart();
            // オープニングカメラのズームアウト機能をONにする
           // Debug.LogWarning("待機だよ");
		}
    }
}

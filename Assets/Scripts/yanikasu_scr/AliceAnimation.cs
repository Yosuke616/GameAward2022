using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliceAnimation : MonoBehaviour
{
	//private ModelAnimation animation;
	private Animator animator;

    int oldAnimState;

    enum eAnimState
    {
        STATE_IDLE = 0,
        STATE_WALK,
        STATE_CLEAR,
        STATE_MISS,
    }

	// Start is called before the first frame update
	void Start()
	{
        oldAnimState = -1;

        // 子オブジェクトからモデルアニメーションコンポーネントを持ってくる
        for (int childNum = 0; childNum < transform.childCount; childNum++)
        {
            // 子オブジェクトの名前検索
            if (transform.GetChild(childNum).name == "Alice_Anim")
            {
                animator = transform.GetChild(childNum).GetComponent<Animator>();
                break;
            }
        }
	}

	// Update is called once per frame
	void Update()
	{
        if (PlayerMove2.AnimState != oldAnimState)
        {
            switch (PlayerMove2.AnimState)
            {
                //case (int)eAnimState.STATE_IDLE: animator.SetAnim("stand-by"); break;
                case (int)eAnimState.STATE_IDLE:
                    animator.CrossFade("stand-by", 0);
                    break;
                case (int)eAnimState.STATE_WALK:
                    animator.CrossFade("walk", 0);
                    break;
                case (int)eAnimState.STATE_CLEAR:
                    animator.CrossFade("clear", 0);
                    break;
                case (int)eAnimState.STATE_MISS:
                    animator.CrossFade("miss", 0);
                    break;
                default:
                    break;
            }
        }

        oldAnimState = PlayerMove2.AnimState;

    }
}

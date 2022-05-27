using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyAnimator : MonoBehaviour
{
    private Animator animator;
    private Fiary_Script fairy;

    int oldAnimState;

    enum eAnimState
    {
        STATE_IDLE = 0,
        STATE_BREAK_START,
        STATE_BREAKING,
        STATE_CLEAR
    }

    // Start is called before the first frame update
    void Start()
    {
        oldAnimState = -1;

        animator = GetComponent<Animator>();
        fairy = GetComponent<Fiary_Script>();


        if (!animator) Debug.LogError("アニメーターを取得できませんでした");
        //animator = GetComponent<ModelAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((int)fairy.GetState != oldAnimState)
        {
            switch ((int)fairy.GetState)
            {
                case (int)eAnimState.STATE_IDLE:
                    animator.CrossFade("Idle", 0);
                    break;
                case (int)eAnimState.STATE_BREAK_START:
                    animator.CrossFade("Take 001", 0);
                    break;
                case (int)eAnimState.STATE_BREAKING:
                    animator.CrossFade("Take 001", 0);
                    break;
                case (int)eAnimState.STATE_CLEAR:
                    animator.CrossFade("Clear", 0);
                    break;
                default:
                    break;
            }
        }

        oldAnimState = (int)fairy.GetState;
    }
}

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

        // �q�I�u�W�F�N�g���烂�f���A�j���[�V�����R���|�[�l���g�������Ă���
        for (int childNum = 0; childNum < transform.childCount; childNum++)
        {
            // �q�I�u�W�F�N�g�̖��O����
            if (transform.GetChild(childNum).name == "Alice_Anim")
            {
                Debug.Log("���f���A�j���[�V�����擾");
                animator = transform.GetChild(childNum).GetComponent<Animator>();
                break;
            }

        }

        if (!animator) Debug.LogError("�A�j���[�^�[���擾�ł��܂���ł���");
        //animator = GetComponent<ModelAnimation>();
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
                    Debug.Log("stand-by");
                    animator.CrossFade("stand-by", 0);
                    break;
                case (int)eAnimState.STATE_WALK:
                    Debug.Log("WALK");
                    animator.CrossFade("walk", 0);
                    break;
                case (int)eAnimState.STATE_CLEAR:
                    Debug.Log("CLEAR");
                    animator.CrossFade("clear", 0);
                    break;
                case (int)eAnimState.STATE_MISS:
                    Debug.Log("MISS");
                    animator.CrossFade("miss", 0);
                    break;
                default:
                    break;
            }
        }

        oldAnimState = PlayerMove2.AnimState;

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //}

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //	animator.SetAnim("walk");
        //}
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //	animator.SetAnim("clear");
        //}
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //	animator.SetAnim("miss");
        //}
    }
}

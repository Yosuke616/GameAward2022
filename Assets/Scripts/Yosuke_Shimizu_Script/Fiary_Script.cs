/*
 2022/3/26 ShimizuYosuke�@�d���̓�������邽�߂̃X�N���v�g
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fiary_Script : MonoBehaviour
{
    public enum eFairyState
    {
        STATE_FOLLOWING_PLAYER = 0,     // �T�u�J�����v���C���[�ɒǏ]
        STATE_DICISION_BREAKING_POINT,  // �j��J�n�ʒu����
        STATE_BREAKING_MOVE,            // �j�蒆

        MAX_FAIRY_STATE
    }

    //����j���Ă��邩�ǂ������m�F���邽�߂̕ϐ�
    CursorSystem CS_Script;
    //�J�[�\���̃V�X�e���𓾂邽�߂̊֐�
    GameObject Cursor_System;

    // �X�e�[�g�̐������v�f���m�ۂ���
    private FairyState[] m_FairyState = new FairyState[(int)eFairyState.MAX_FAIRY_STATE];
    // ���݂̃X�e�[�g
    [SerializeField] private eFairyState m_CurrentState;
    public eFairyState GetState { get { return m_CurrentState; } }
    public void SetState(eFairyState state)
    {
        m_CurrentState = state;
    }


    // ����������t���O
    private bool small, big;
    Vector3 normalScale;
    int frameCount;
    public void SmallStart()
    {
        small = true;
        frameCount = 30;
    }
    public void BigStart()
    {
        big = true;
        frameCount = 30;
    }
    public void Back()
    {
        this.transform.Rotate(new Vector3(0, 90, 0));
    }
    public void Front()
    {
        this.transform.Rotate(new Vector3(0, 90, 0));
    }


    void Start()
    {
        big = small = false;
        normalScale = new Vector3(15.0f, 15.0f, 15.0f);
        frameCount = 30;

        //�j���Ă��邩�ǂ����̃t���O��������悤�ȃJ�[�\���𓾂�
        Cursor_System = GameObject.Find("Cursor");
        //�X�N���v�g�𓾂�
        CS_Script = Cursor_System.GetComponent<CursorSystem>();

        // �ŏ��̓v���C���[�Ǐ]
        m_CurrentState = eFairyState.STATE_FOLLOWING_PLAYER;

        // �e�X�e�[�g��z��ɓ���Ă���(AddComponent�̂悤�Ȃ���)
        // �v���C���[�Ǐ]
        m_FairyState[(int)eFairyState.STATE_FOLLOWING_PLAYER] = FollowingPlayer.Instantiate(this.gameObject);
        // �j����W�̌���
        m_FairyState[(int)eFairyState.STATE_DICISION_BREAKING_POINT] = DicisionBreakingPoint.Instantiate(this.gameObject);
        // �j�蒆
        m_FairyState[(int)eFairyState.STATE_BREAKING_MOVE] = BreakingMove.Instantiate(this.gameObject);


    }

    void Update()
    {
        //�v���C���[�̏󋵂𓾂�
        GameObject player = GameObject.Find("ParentPlayer");

        if (player.GetComponent<PlayerMove2>().GetFlg() && player.GetComponent<PlayerMove2>().GetGameOverFlg())
        {
            // �X�e�[�g�ɉ����čX�V���e��ς���
            m_FairyState[(int)m_CurrentState].UpdateFairy();

            if (small)
            {
                // 0.5�b�ŃX�P�[����0�ɂ���
                frameCount--;

                Vector3 scale = normalScale * (frameCount / 30.0f);

                this.transform.localScale = scale;

                if (frameCount <= 0)
                {
                    small = false;
                    frameCount = 30;
                }
            }
            else if (big)
            {
                // 0.5�b�ŃX�P�[����0�ɂ���
                frameCount--;

                Vector3 scale = normalScale * (Mathf.Abs((float)(30 - frameCount)) / 30.0f);

                this.transform.localScale = scale;

                if (frameCount <= 0)
                {
                    big = false;
                    frameCount = 30;
                }
            }

        }
        else
        {
            //this.gameObject.SetActive(false);
        }
    }

}

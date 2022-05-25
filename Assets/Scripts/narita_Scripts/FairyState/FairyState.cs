using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyState : MonoBehaviour
{
    public enum eFairyState
    {
        STATE_FOLLOWING_PLAYER = 0,     // �T�u�J�����v���C���[�ɒǏ]
        STATE_DICISION_BREAKING_POINT,  // �j��J�n�ʒu����
        STATE_BREAKING_MOVE,            // �j�蒆

        MAX_FAIRY_STATE
    }

    static private eFairyState m_eFairyState;
    static public eFairyState GetState { get { return m_eFairyState; } }

    void Start()
    {
        // �v���C���[�ɒǏ]
        m_eFairyState = eFairyState.STATE_FOLLOWING_PLAYER;
    }

    void Update()
    {
        
    }

    public static void SetState(eFairyState state)
    {
        m_eFairyState = state;
    }

    public virtual void UpdateFairy(){}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyState : MonoBehaviour
{
    public enum eFairyState
    {
        STATE_FOLLOWING_PLAYER = 0,     // サブカメラプレイヤーに追従
        STATE_DICISION_BREAKING_POINT,  // 破り開始位置決定
        STATE_BREAKING_MOVE,            // 破り中

        MAX_FAIRY_STATE
    }

    static private eFairyState m_eFairyState;
    static public eFairyState GetState { get { return m_eFairyState; } }

    void Start()
    {
        // プレイヤーに追従
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

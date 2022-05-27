using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLotate : MonoBehaviour
{
    enum eDirection
    {
        RIGHT = 0,
        LEFT,
    }

    [SerializeField] private eDirection m_dir;
    // 右回転の限界
    const float rightLotLimit = 120.0f;
    const float leftLotLimit  = 240.0f;
    // 1フレームの回転量
    const float RATE_ROTATE_MODEL = 5.0f;

    private PlayerMove2 playerCom;

    void Start()
    {
        playerCom = GameObject.Find("ParentPlayer").GetComponent<PlayerMove2>();
        m_dir = eDirection.RIGHT;
    }

    void Update()
    {
        if (CursorSystem.GetGameState() == CursorSystem.GameState.MODE_OPENING) return;
        if (playerCom.GetFlg() && playerCom.GetGameOverFlg())
        {
            // 左向き
            if (Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") == -1)
            {
                m_dir = eDirection.LEFT;
            }
            // 右向き
            else if (Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") == 1)
            {
                m_dir = eDirection.RIGHT;
            }

            switch (m_dir)
            {
                case eDirection.RIGHT: // 右向き
                    if (transform.localEulerAngles.y > rightLotLimit)
                    {
                        transform.Rotate(new Vector3(0, -RATE_ROTATE_MODEL, 0));
                    }
                    break;
                case eDirection.LEFT:  // 左向き
                    if (transform.localEulerAngles.y < leftLotLimit)
                    {
                        transform.Rotate(new Vector3(0, RATE_ROTATE_MODEL, 0));
                    }
                    //else
                    //{
                    //    transform.Rotate(new Vector3(0, leftLotLimit - transform.rotation.y, 0));
                    //}
                    break;
                default:
                    break;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSMove : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.6f;

    private float minusNum;

    private Vector3 pos;

    private bool moveLeft, moveRight;

    private Vector3 tmp, targetPos;

    // Start is called before the first frame update
    void Start()
    {
        moveLeft = false;
        moveRight = false;

        tmp = gameObject.transform.position;
        targetPos = new Vector3(-CreateTriangle.paperSizeX * 2.0f - 2.0f, tmp.y, tmp.z);
        pos = new Vector3(0.0f, tmp.y, tmp.z);
        minusNum = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveLeft)
        {
            // オープニング中は移動させない
            if (CursorSystem.GetGameState() == CursorSystem.GameState.MODE_OPENING)
            {
                moveLeft = false;
                return;
            }

            // 紙の移動量
            pos.x += moveSpeed;

            // 紙の座標移動
            transform.Translate(Vector3.left * pos.x * Time.deltaTime);
            if (transform.position.x <= targetPos.x)
            {
                transform.position = targetPos;
                pos = targetPos;
                moveLeft = false;
                Debug.Log(pos.x);
            }
        }
        if (moveRight)
        {
            // オープニング中は移動させない
            if (CursorSystem.GetGameState() == CursorSystem.GameState.MODE_OPENING)
            {
                moveRight = false;
                return;
            }

            // 紙の移動量
            //if (pos.x < 0.0f)
            //    pos.x *= -1.0f;
            if (pos.x >= 20.0f)
            {
                pos.x -= 0.1f;
            }
            else if (pos.x >= 12.0f)
            {
                pos.x -= 0.2f;
            }
            else
            {
                pos.x -= 0.4f;
            }
            if (pos.x < 0.0f)
                pos.x *= -1.0f;

            // 紙の移動
            transform.Translate(Vector3.right * pos.x * Time.deltaTime);
            if (transform.position.x >= tmp.x)
            {
                transform.position = tmp;
                pos = new Vector3(0.0f, tmp.y, tmp.z); ;
                minusNum = 0.0f;
                moveRight = false;
                //一番手前の紙の時に破るモード
                if (GetComponent<DivideTriangle>().GetNumber() == 1) {
                    CursorSystem.SetGameState(CursorSystem.GameState.MODE_ACTION);
                }
            }
        }
    }

    public bool StartLeft()
    {
        if (!moveRight)
        {
            moveLeft = true;
            return true;
        }

        return false;
    }

    public bool StartRight()
    {
        if (!moveLeft)
        {
            moveRight = true;
            return true;
        }

        return false;
    }
}

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
            Debug.Log("左");

            pos.x += moveSpeed;

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
            Debug.Log("右");

            if (pos.x < 0.0f)
                pos.x *= -1.0f;
            //if(pos.x >= 17.8f)
            //{
            //    pos.x -= moveSpeed / 8.0f;
            //}
            //else
            //{
            //    pos.x -= moveSpeed * 3.0f;
            //}
            //minusNum += 0.5f;
            //pos.x -= moveSpeed / 3.3f;
            if (pos.x >= 11.0f)
            {
                pos.x -= 0.17f;
            }
            else if (pos.x >= 1.0f)
            {
                pos.x -= 0.28f;
            }
            else
            {
                pos.x -= 1.0f;
            }
            //pos.x -= 0.186f;

            Debug.Log(pos.x);

            transform.Translate(Vector3.right * pos.x * Time.deltaTime);
            if (transform.position.x >= tmp.x)
            {
                transform.position = tmp;
                pos = new Vector3(0.0f, tmp.y, tmp.z); ;
                minusNum = 0.0f;
                moveRight = false;

                // めくるモード → アクションモード
                //CursorSystem.SetGameState(CursorSystem.GameState.MODE_ACTION);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSMove : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 12.0f;

    private bool moveLeft, moveRight;

    private Vector3 tmp, targetPos;

    // Start is called before the first frame update
    void Start()
    {
        moveLeft = false;
        moveRight = false;

        tmp = gameObject.transform.position;
        targetPos = new Vector3(-CreateTriangle.paperSizeX * 2.0f - 2.0f, tmp.y, tmp.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (moveLeft)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            if (transform.position.x <= targetPos.x)
            {
                transform.position = targetPos;
                moveLeft = false;
            }
        }
        if (moveRight)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            if (transform.position.x >= tmp.x)
            {
                transform.position = tmp;
                moveRight = false;
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

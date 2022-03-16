/*
 2022/2/28 志水陽祐

 2022/3/02 相羽星那・・・左右の自動移、下矢印で止まる


 プレイヤーの操作と地面の設定
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update

    //キャラクターの操作状況の管理
    [SerializeField] public bool onGround = true;
    //[SerializeField] public bool inJumping = false;

    //rigidbodyオブジェクト格納用変数
    //Rigidbody rb;

    public enum EMoveCharacter
    {
        STOP_MOVE = 0,
        RIGHT_MOVE,
        LEFT_MOVE,


        MAX_MOVE
    }


    //速度
    public float speed = 5.0f;
    public float jump = 5.0f;

    private Rigidbody rb;

    public EMoveCharacter eCharaMove;
	public bool isjump;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        eCharaMove = EMoveCharacter.STOP_MOVE;
        isjump = false;
        //rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
		if ((Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("Vertical") > 0) && !isjump && onGround)
		{
			rb.velocity = Vector3.up * jump;
			isjump = true;
			onGround = false;
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Horizontal") < 0)
		{
			eCharaMove = EMoveCharacter.RIGHT_MOVE;
			//transform.position += transform.right * speed * Time.deltaTime;
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") > 0)
		{
			eCharaMove = EMoveCharacter.LEFT_MOVE;
			//transform.position -= transform.right * speed * Time.deltaTime;
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0)
		{
			eCharaMove = EMoveCharacter.STOP_MOVE;
		}

        switch (eCharaMove)
        {
            case EMoveCharacter.STOP_MOVE:
                break;
            case EMoveCharacter.RIGHT_MOVE:
                transform.position += transform.right * speed * Time.deltaTime;
                break;
            case EMoveCharacter.LEFT_MOVE:
                transform.position -= transform.right * speed * Time.deltaTime;
                break;

        }

        //移動の実行
        //if (!inJumping) {   //空中での移動を禁止
        //transform.position += transform.forward * fv;
        //}

        //スペースでジャンプする
        //if (onGround)
        //{
        //    if (Input.GetKey(KeyCode.Space))
        //    {
        //        //ジャンプさせるために上方向の力を加える
        //        rb.AddForce(transform.up * 8, ForceMode.Impulse);

        //        //ジャンプ中の地面の接触判定はfalseに変える
        //        onGround = false;
        //        inJumping = true;

        //        //左右キー押しながらジャンプしたときは左右方向に力を加える
        //        if (Input.GetKey(KeyCode.RightArrow)) {
        //            rb.AddForce(transform.forward * 6.0f, ForceMode.Impulse);
        //        } else if (Input.GetKey(KeyCode.LeftArrow)) {
        //            rb.AddForce(transform.forward * -3.0f, ForceMode.Impulse);
        //        }

        //    }
        //}
    }

    //地面に接触したらonGroundをtrue、inJumpingをfalseにする
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ground")
        {
			Debug.Log($"CollisionEnter : Ground");
            onGround = true;
			isjump = false;
        }
    }
}

/*
 2022/2/28 志水陽祐
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
    [SerializeField] public bool inJumping = false;

    //rigidbodyオブジェクト格納用変数
    Rigidbody rb;

    //移動速度の定義(数値を変えてスピードを変える) 
    float Speed = 6.0f;

    //ダッシュ速度の定義
    float SprintSpeed = 9.0f;

    //移動の係数格納用変数の定義
    float fv;

    void Start()
    {
        //プレイヤーのrigidbodyを持ってくる
        rb = this.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        //Shift+左右キーでダッシュ、左右で歩き
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftShift)) {          //右ダッシュ
            fv = Time.deltaTime * SprintSpeed;
        }else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.LeftShift)){      //左ダッシュ
            fv = -Time.deltaTime * SprintSpeed;
        }else if (Input.GetKey(KeyCode.RightArrow)){        //右歩き
            fv = Time.deltaTime * Speed;
        }else if (Input.GetKey(KeyCode.LeftArrow)){         //左歩き
            fv = -Time.deltaTime * Speed;
        }else {
            fv = 0;
        }

        //移動の実行
        //if (!inJumping) {   //空中での移動を禁止
            transform.position += transform.forward * fv;
        //}

        //スペースでジャンプする
        if (onGround) {
            if (Input.GetKey(KeyCode.Space)) {
                //ジャンプさせるために上方向の力を加える
                rb.AddForce(transform.up * 8, ForceMode.Impulse);

                //ジャンプ中の地面の接触判定はfalseに変える
                onGround = false;
                inJumping = true;

                //左右キー押しながらジャンプしたときは左右方向に力を加える
                if (Input.GetKey(KeyCode.RightArrow)) {
                    rb.AddForce(transform.forward * 6.0f, ForceMode.Impulse);
                } else if (Input.GetKey(KeyCode.LeftArrow)) {
                    rb.AddForce(transform.forward * -3.0f, ForceMode.Impulse);
                }

            }
        }
    }

    //地面に接触したらonGroundをtrue、inJumpingをfalseにする
    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "Ground") {
            onGround = true;
            inJumping = false;
        }
    }
}

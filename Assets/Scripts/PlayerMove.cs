/*
 2022/02/28 志水陽祐

 2022/03/02 相羽星那	左右の自動移、下矢印で止まる

 2022/03/21 酒井諒太郎	カメラの角度に合わせてプレイヤーの方角を決定するよう変更

 プレイヤーの操作と地面の設定
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	[SerializeField] public bool onGround;					// 陸上か空中か
	[SerializeField] private Vector3 velocity;              // 移動方向
	[SerializeField] private float moveSpeed = 0.01f;        // 移動速度
	[SerializeField] private float applySpeed = 0.2f;       // 振り向きの適用速度
	[SerializeField] private float jump = 5.0f;
	[SerializeField] private Camera refCamera;  // カメラの水平回転を参照する用

    private Rigidbody rb;
	//private ModelAnimation animation;

    public EMoveCharacter eCharaMove;
	public bool isjump;

	Camera mainCamera;
	public enum EMoveCharacter
    {
        STOP_MOVE = 0,
        RIGHT_MOVE,
        LEFT_MOVE,


        MAX_MOVE
    }

    void Start()
    {
		onGround = true;
		rb = GetComponent<Rigidbody>();
		//animation = GetComponent<ModelAnimation>();
        eCharaMove = EMoveCharacter.STOP_MOVE;
        isjump = false;
		mainCamera = Camera.main;
        //rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetAxis("Vertical") > 0)
        //{
        //    Debug.LogWarning("");
        //}

		velocity = Vector3.zero;
		if ((Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("Vertical") > 0) && !isjump && onGround)
		{
			rb.velocity = Vector3.up * jump;
			isjump = true;
			onGround = false;
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Horizontal") < 0)
		{
			eCharaMove = EMoveCharacter.RIGHT_MOVE;
            Debug.LogWarning("RIGHT_MOVE");
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") > 0)
		{
			eCharaMove = EMoveCharacter.LEFT_MOVE;
            Debug.LogWarning("LEFT_MOVE");
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0)
		{
			eCharaMove = EMoveCharacter.STOP_MOVE;
            Debug.LogWarning("STOP_MOVE");
        }

        switch (eCharaMove)
        {
            case EMoveCharacter.RIGHT_MOVE:
				//animation.SetAnim("Walk");
				velocity.x -= 1;
                break;
            case EMoveCharacter.LEFT_MOVE:
				//animation.SetAnim("Walk");
				velocity.x += 1;
				break;
            case EMoveCharacter.STOP_MOVE:
				//animation.SetAnim("Stand-by");
				velocity.x = 0;
				break;
        }

		// 速度ベクトルの長さを1秒でmoveSpeedだけ進むように調整します
		velocity = velocity.normalized * moveSpeed * Time.deltaTime;

		// いずれかの方向に移動している場合
		if (velocity.magnitude > 0)
		{
			// プレイヤーの回転(transform.rotation)の更新
			// 無回転状態のプレイヤーのZ+方向(後頭部)を、
			// カメラの水平回転(refCamera.hRotation)で回した移動の反対方向(-velocity)に回す回転に段々近づけます
			transform.rotation = Quaternion.Slerp(transform.rotation,
												  Quaternion.LookRotation(Quaternion.identity * velocity),
												  applySpeed);

			// プレイヤーの位置(transform.position)の更新
			// カメラの水平回転(refCamera.hRotation)で回した移動方向(velocity)を足し込みます
			transform.position += Quaternion.identity * velocity;
		}

		//移動の実行
		if (!isjump)
		{   //空中での移動を禁止
		}

		//スペースでジャンプする
		if (onGround)
		{
			if (Input.GetKey(KeyCode.Space))
			{
				//ジャンプさせるために上方向の力を加える
				rb.AddForce(transform.up * 8, ForceMode.Impulse);

				//ジャンプ中の地面の接触判定はfalseに変える
				onGround = false;
				isjump = true;

				//左右キー押しながらジャンプしたときは左右方向に力を加える
				if (Input.GetKey(KeyCode.RightArrow))
				{
					rb.AddForce(transform.forward * 6.0f, ForceMode.Impulse);
				}
				else if (Input.GetKey(KeyCode.LeftArrow))
				{
					rb.AddForce(transform.forward * -3.0f, ForceMode.Impulse);
				}
			}
		}
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBehavior : MonoBehaviour
{
	enum ENEMY_MODE
	{
		ENEMY_NONE,
		ENEMY_MOVE,
		ENEMY_DISCOVER,
	}
	public float MaxMovePos = 10.0f;
	public float EnemyViewLength = 5.0f;
	public float MoveSpeed = 0.1f;
	public bool StartMoveLeft = true;


	[SerializeField] private ENEMY_MODE nEnemyMode;
	private bool bLeft;
	private Vector3 StartPos;
	private GameObject ObjPlayer;

	void Start()
	{
		//nEnemyMode = ENEMY_MODE.ENEMY_MOVE;
		nEnemyMode = ENEMY_MODE.ENEMY_NONE;
		bLeft = StartMoveLeft;
		StartPos = transform.position;
		Debug.Log($"EnemyMode:{nEnemyMode}");
		ObjPlayer = transform.parent.gameObject.transform.Find("d1").gameObject;	// Enemyの親オブジェクト(SubCameraX)の子オブジェクトの中からPlayerを検索
	}

    void Update()
	{
		switch (nEnemyMode)
		{
			// 警戒、警備、移動
			case ENEMY_MODE.ENEMY_MOVE:

				// 移動処理
				if (bLeft)
					transform.position +=  Vector3.right * MoveSpeed;
				else
					transform.position += -Vector3.right * MoveSpeed;

				// 初期位置から指定距離以上離れた場合の左右移動切替
				if (Vector3.Distance(transform.position, StartPos) >= MaxMovePos)
					bLeft = !bLeft;

				// 現在地から指定距離以内にプレイヤーがいた場合の処理
				if (Vector3.Distance(transform.position, ObjPlayer.transform.position) <= EnemyViewLength)
				{
					nEnemyMode = ENEMY_MODE.ENEMY_DISCOVER;
					Debug.Log($"EnemyMode:{nEnemyMode}");
				}

				break;


			// 発見
			case ENEMY_MODE.ENEMY_DISCOVER:
				Vector3 dir = (ObjPlayer.transform.position - this.transform.position).normalized;
				this.transform.Translate(dir.x * MoveSpeed, 0, dir.z * MoveSpeed);
				break;


			default:
				break;
		}
	}


    // あたり判定オブジェクトと実際に見えているオブジェクトの座標を合わせる
    public void Synchronous(GameObject gameObject)
    {
        Vector3 distance;
        distance = transform.position - StartPos;

        gameObject.transform.Translate(distance);
    }

    public void Active()
    {
        nEnemyMode = ENEMY_MODE.ENEMY_MOVE;
    }
}

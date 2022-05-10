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


	private ENEMY_MODE nEnemyMode;
	private bool bLeft;
	private Vector3 StartPos;
	private GameObject ObjPlayer;

	void Start()
	{
		nEnemyMode = ENEMY_MODE.ENEMY_MOVE;
		bLeft = StartMoveLeft;
		StartPos = transform.position;
		Debug.Log($"EnemyMode:{nEnemyMode}");
		ObjPlayer = transform.parent.gameObject.transform.Find("d1").gameObject;	// Enemy�̐e�I�u�W�F�N�g(SubCameraX)�̎q�I�u�W�F�N�g�̒�����Player������
	}

    void Update()
	{
		switch (nEnemyMode)
		{
			// �x���A�x���A�ړ�
			case ENEMY_MODE.ENEMY_MOVE:

				// �ړ�����
				if (bLeft)
					transform.position +=  Vector3.right * MoveSpeed;
				else
					transform.position += -Vector3.right * MoveSpeed;

				// �����ʒu����w�苗���ȏ㗣�ꂽ�ꍇ�̍��E�ړ��ؑ�
				if (Vector3.Distance(transform.position, StartPos) >= MaxMovePos)
					bLeft = !bLeft;

				// ���ݒn����w�苗���ȓ��Ƀv���C���[�������ꍇ�̏���
				if (Vector3.Distance(transform.position, ObjPlayer.transform.position) <= EnemyViewLength)
				{
					nEnemyMode = ENEMY_MODE.ENEMY_DISCOVER;
					Debug.Log($"EnemyMode:{nEnemyMode}");
				}

				break;


			// ����
			case ENEMY_MODE.ENEMY_DISCOVER:
				Vector3 dir = (ObjPlayer.transform.position - this.transform.position).normalized;
				this.transform.Translate(dir.x * MoveSpeed, 0, dir.z * MoveSpeed);
				break;


			default:
				break;
		}
	}
}

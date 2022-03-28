/*
 2022/02/28 �u���z�S

 2022/03/02 ���H����	���E�̎����ځA�����Ŏ~�܂�

 2022/03/21 ����ȑ��Y	�J�����̊p�x�ɍ��킹�ăv���C���[�̕��p�����肷��悤�ύX

 �v���C���[�̑���ƒn�ʂ̐ݒ�
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	[SerializeField] public bool onGround;					// ���ォ�󒆂�
	[SerializeField] private Vector3 velocity;              // �ړ�����
	[SerializeField] private float moveSpeed = 0.01f;        // �ړ����x
	[SerializeField] private float applySpeed = 0.2f;       // �U������̓K�p���x
	[SerializeField] private float jump = 5.0f;
	[SerializeField] private Camera refCamera;  // �J�����̐�����]���Q�Ƃ���p

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

		// ���x�x�N�g���̒�����1�b��moveSpeed�����i�ނ悤�ɒ������܂�
		velocity = velocity.normalized * moveSpeed * Time.deltaTime;

		// �����ꂩ�̕����Ɉړ����Ă���ꍇ
		if (velocity.magnitude > 0)
		{
			// �v���C���[�̉�](transform.rotation)�̍X�V
			// ����]��Ԃ̃v���C���[��Z+����(�㓪��)���A
			// �J�����̐�����](refCamera.hRotation)�ŉ񂵂��ړ��̔��Ε���(-velocity)�ɉ񂷉�]�ɒi�X�߂Â��܂�
			transform.rotation = Quaternion.Slerp(transform.rotation,
												  Quaternion.LookRotation(Quaternion.identity * velocity),
												  applySpeed);

			// �v���C���[�̈ʒu(transform.position)�̍X�V
			// �J�����̐�����](refCamera.hRotation)�ŉ񂵂��ړ�����(velocity)�𑫂����݂܂�
			transform.position += Quaternion.identity * velocity;
		}

		//�ړ��̎��s
		if (!isjump)
		{   //�󒆂ł̈ړ����֎~
		}

		//�X�y�[�X�ŃW�����v����
		if (onGround)
		{
			if (Input.GetKey(KeyCode.Space))
			{
				//�W�����v�����邽�߂ɏ�����̗͂�������
				rb.AddForce(transform.up * 8, ForceMode.Impulse);

				//�W�����v���̒n�ʂ̐ڐG�����false�ɕς���
				onGround = false;
				isjump = true;

				//���E�L�[�����Ȃ���W�����v�����Ƃ��͍��E�����ɗ͂�������
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

	//�n�ʂɐڐG������onGround��true�AinJumping��false�ɂ���
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

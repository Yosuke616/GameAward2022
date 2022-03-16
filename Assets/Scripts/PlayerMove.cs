/*
 2022/2/28 �u���z�S

 2022/3/02 ���H���߁E�E�E���E�̎����ځA�����Ŏ~�܂�


 �v���C���[�̑���ƒn�ʂ̐ݒ�
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update

    //�L�����N�^�[�̑���󋵂̊Ǘ�
    [SerializeField] public bool onGround = true;
    //[SerializeField] public bool inJumping = false;

    //rigidbody�I�u�W�F�N�g�i�[�p�ϐ�
    //Rigidbody rb;

    public enum EMoveCharacter
    {
        STOP_MOVE = 0,
        RIGHT_MOVE,
        LEFT_MOVE,


        MAX_MOVE
    }


    //���x
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

        //�ړ��̎��s
        //if (!inJumping) {   //�󒆂ł̈ړ����֎~
        //transform.position += transform.forward * fv;
        //}

        //�X�y�[�X�ŃW�����v����
        //if (onGround)
        //{
        //    if (Input.GetKey(KeyCode.Space))
        //    {
        //        //�W�����v�����邽�߂ɏ�����̗͂�������
        //        rb.AddForce(transform.up * 8, ForceMode.Impulse);

        //        //�W�����v���̒n�ʂ̐ڐG�����false�ɕς���
        //        onGround = false;
        //        inJumping = true;

        //        //���E�L�[�����Ȃ���W�����v�����Ƃ��͍��E�����ɗ͂�������
        //        if (Input.GetKey(KeyCode.RightArrow)) {
        //            rb.AddForce(transform.forward * 6.0f, ForceMode.Impulse);
        //        } else if (Input.GetKey(KeyCode.LeftArrow)) {
        //            rb.AddForce(transform.forward * -3.0f, ForceMode.Impulse);
        //        }

        //    }
        //}
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

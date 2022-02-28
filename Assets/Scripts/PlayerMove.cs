/*
 2022/2/28 �u���z�S
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
    [SerializeField] public bool inJumping = false;

    //rigidbody�I�u�W�F�N�g�i�[�p�ϐ�
    Rigidbody rb;

    //�ړ����x�̒�`(���l��ς��ăX�s�[�h��ς���) 
    float Speed = 6.0f;

    //�_�b�V�����x�̒�`
    float SprintSpeed = 9.0f;

    //�ړ��̌W���i�[�p�ϐ��̒�`
    float fv;

    void Start()
    {
        //�v���C���[��rigidbody�������Ă���
        rb = this.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        //Shift+���E�L�[�Ń_�b�V���A���E�ŕ���
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftShift)) {          //�E�_�b�V��
            fv = Time.deltaTime * SprintSpeed;
        }else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.LeftShift)){      //���_�b�V��
            fv = -Time.deltaTime * SprintSpeed;
        }else if (Input.GetKey(KeyCode.RightArrow)){        //�E����
            fv = Time.deltaTime * Speed;
        }else if (Input.GetKey(KeyCode.LeftArrow)){         //������
            fv = -Time.deltaTime * Speed;
        }else {
            fv = 0;
        }

        //�ړ��̎��s
        //if (!inJumping) {   //�󒆂ł̈ړ����֎~
            transform.position += transform.forward * fv;
        //}

        //�X�y�[�X�ŃW�����v����
        if (onGround) {
            if (Input.GetKey(KeyCode.Space)) {
                //�W�����v�����邽�߂ɏ�����̗͂�������
                rb.AddForce(transform.up * 8, ForceMode.Impulse);

                //�W�����v���̒n�ʂ̐ڐG�����false�ɕς���
                onGround = false;
                inJumping = true;

                //���E�L�[�����Ȃ���W�����v�����Ƃ��͍��E�����ɗ͂�������
                if (Input.GetKey(KeyCode.RightArrow)) {
                    rb.AddForce(transform.forward * 6.0f, ForceMode.Impulse);
                } else if (Input.GetKey(KeyCode.LeftArrow)) {
                    rb.AddForce(transform.forward * -3.0f, ForceMode.Impulse);
                }

            }
        }
    }

    //�n�ʂɐڐG������onGround��true�AinJumping��false�ɂ���
    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "Ground") {
            onGround = true;
            inJumping = false;
        }
    }
}

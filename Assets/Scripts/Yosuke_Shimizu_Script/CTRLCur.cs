using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTRLCur : MonoBehaviour
{
    private Vector3 SendPos;

    // Start is called before the first frame update
    void Start()
    {
        //�������ŃI�u�W�F�N�g�̏ꏊ�����߂�
        this.transform.position = new Vector3(0.0f,0.0f,0.0f);
        SendPos = new Vector3(0.0f,0.0f,0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //�E�����ɓ�����
        if (Input.GetAxis("Horizontal2") < 0 && this.transform.position.x < 9.0f) {
            this.transform.position += new Vector3(0.25f,0.0f,0.0f);
        }
        //�������ɓ�����
        if (Input.GetAxis("Horizontal2") > 0 && this.transform.position.x > -9.0f)
        {
            this.transform.position += new Vector3(-0.25f, 0.0f, 0.0f);
        }
        //�������ɓ�����
        if (Input.GetAxis("Vertical2") < 0 && this.transform.position.y > -6.0f) {
            this.transform.position += new Vector3(0.0f, -0.25f, 0.0f);
        }
        //������ɓ�����
        if (Input.GetAxis("Vertical2") > 0 && this.transform.position.y < 6.0f) {
            this.transform.position += new Vector3(0.0f, 0.25f, 0.0f);
        }

        SendPos = this.transform.position;
    }

    //�J�[�\���𓮂������߂ɂ����̍��W�𑗂�
    public Vector3 GetCTRLPos() {
        return SendPos;
    }
}

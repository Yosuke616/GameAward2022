/*
�쐬���F2022/5/07 Shimizu Shogo
���e  �F�؂���ꂽ����ό`������
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BreakingPaper : MonoBehaviour
{
    private Material _mat;              // �}�e���A��

    public float Add;                   // �ω�������l
    public float valueChange;           // Shader�ɓn���l
    public float valueAngle;            // �p�x(�����E��)
    public float valueRadius;           // �Œ�l
    public float valueStrength;         // �Œ�l

    private bool bStart = false;        // ����t���O
    private bool bVibration = false;    // �U���t���O

    const float TURN_OVER = 1.4f;       // �߂���X�s�[�h

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // �Q�[���p�b�h�󋵎擾
        var gamepad = Gamepad.current;

        // �j��鏈��
        if (bStart)
        {
            // ���񂾂�߂���Ă���
            valueChange += Add;
            Debug.Log(valueChange);

            // �U������
            if(gamepad != null && !bVibration)
            {
                StartCoroutine("Vibration");
                bVibration = true;
            }

            if (valueChange >= 300.0f)
            {
                valueChange = 300.0f;
                bStart = false;
                bVibration = false;
            }
            else if (valueChange <= -300.0f)
            {
                valueChange = -300.0f;
                bStart = false;
                bVibration = false;
            }
            _mat.SetFloat("_BendPivot", valueChange);
            _mat.SetFloat("_BendAngle", valueAngle);
            _mat.SetFloat("_BendRadius", valueRadius);
            _mat.SetFloat("_BendStrength", valueStrength);
            _mat = GetComponent<Renderer>().sharedMaterial;
        }
    }

    // �E���ɂ߂���
    public void SetRight()
    {
        bStart = true;
        //Add = 0.7f;
        Add = TURN_OVER;
        //valueChange = -30.0f;
        //valueAngle = 220.0f;
        //valueAngle = 135.0f;
        //valueRadius = 14.9f;
        //valueStrength = 1.0f;

        valueChange = -30.0f;
        valueAngle = 225.0f;
        valueRadius = 14.9f;
        valueStrength = 1.0f;
        //gameObject.transform.position;
    }

    // �����ɂ߂���
    public void SetLeft()
    {
        bStart = true;
        //Add = 0.7f;
        Add = TURN_OVER;
        //valueChange = -30.0f;
        //valueAngle = 225.0f;
        //valueRadius = 14.9f;
        //valueStrength = 1.0f;

        valueChange = -30.0f;
        valueAngle = 135.0f;
        valueRadius = 14.9f;
        valueStrength = 1.0f;
        
    }

    // �}�e���A���̐ݒ�
    public void SetMaterial(Material mat)
    {
        GetComponent<Renderer>().material = mat;
        _mat = GetComponent<Renderer>().sharedMaterial;
    }

    IEnumerator Vibration()
    {
        // �Q�[���p�b�h�̏�Ԏ擾
        var game = Gamepad.current;
        
        // �U���̋���
        game.SetMotorSpeeds(0.6f, 0.6f);
        
        // �U���b��
        yield return new WaitForSecondsRealtime(1.0f);
        
        // �U������
        game.SetMotorSpeeds(0.0f, 0.0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // �V�[���J�ڗp

public class StageSelect : MonoBehaviour
{
    // ���݂̐i��
    public static int ProgressStage = 0;

    // ���C���J�����I��
    public Camera camera;

    // �J�����ړ����x�i�X�s�[�h�j
    public float SpeedCamera;

    // �J�����ړ��ʔ͈�
    public float RangeCamera;

    // �J�����ړ���
    private float MoveCamera = 0;

    // �J�������
    enum CAMERA_STATE
    {
        LEFT,
        RIGHT,
        NONE
    }
    private CAMERA_STATE _STATE = CAMERA_STATE.NONE;

    // �����݂̑I���p�l��
    private int Select = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // �i���󋵂ɂ���čŏ��̏󋵕ω��i�N���A���o�܂ށj
        //switch(ProgressStage)
        //{
        //}

        // �X�e�[�W�I��
        if (_STATE == CAMERA_STATE.NONE)
        {
            // ����ʈړ�
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (Select > 0)
                {
                    Select--;
                    _STATE = CAMERA_STATE.LEFT;
                }
                else
                    Select = 0;
            }

            // ����ʈړ�
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (Select < ProgressStage)
                {
                    Select++;
                    _STATE = CAMERA_STATE.RIGHT;
                }
                else
                    Select = ProgressStage;
            }
        }

        switch (_STATE)
        {
            case CAMERA_STATE.LEFT:
                camera.transform.position -= new Vector3(SpeedCamera, 0, 0);
                MoveCamera += SpeedCamera;
                if (MoveCamera >= RangeCamera)
                {
                    MoveCamera = 0;
                    _STATE = CAMERA_STATE.NONE;
                }
                break;

            case CAMERA_STATE.RIGHT:
                camera.transform.position += new Vector3(SpeedCamera, 0, 0);
                MoveCamera += SpeedCamera;
                if (MoveCamera >= RangeCamera)
                {
                    MoveCamera = 0;
                    _STATE = CAMERA_STATE.NONE;
                }
                break;

            case CAMERA_STATE.NONE:
                break;
        }

            // �X�e�[�W�˓�
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (Select)
            {
                case 0:
                SceneManager.LoadScene("test_Stage1");
                break;

                case 1:
                SceneManager.LoadScene("1-2");
                break;

                case 2:
                SceneManager.LoadScene("1-3");
                break;

                case 3:
                SceneManager.LoadScene("1-4");
                break;
            }
        }
    }


    // �X�e�[�W�N���A���ɐi���ۑ��֐�
    public static void UpdateProgress(string name)
    {
        if (name == "test_Stage1") 
        {
            if (ProgressStage <= 1)
                ProgressStage = 1;
        }

        if (name == "1-2")
        {
            if (ProgressStage <= 2)
                ProgressStage = 2;
        }

        if (name == "1-3")
        {
            if (ProgressStage <= 3)
                ProgressStage = 3;
        }

        if (name == "1-4")
        {
            if (ProgressStage <= 4)
                ProgressStage = 4;
        }
    }
}

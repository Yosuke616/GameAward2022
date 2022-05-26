using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{

    [SerializeField]
    private int minute;
    [SerializeField]
    private float seconds;

    //�@�O��Update�̎��̕b��
    private float oldSeconds;
    //�@�^�C�}�[�\���p�e�L�X�g
    private Text timerText;
    // �^�C�}�[�J�n�p�t���O
    private bool bStart;

    //���v�̎���
    private float SumTime;

    void Start()
    {
        minute = 0;
        seconds = 0f;
        oldSeconds = 0f;
        timerText = GetComponentInChildren<Text>();
        bStart = false;
        gameObject.GetComponent<Text>().enabled = bStart;
    }

    void Update()
    {
        GameObject player = GameObject.Find("ParentPlayer");
        if (gameObject.GetComponent<Text>().enabled)
        {
            if (player.GetComponent<PlayerMove2>().GetFlg() && player.GetComponent<PlayerMove2>().GetGameOverFlg())
            {
                seconds += Time.deltaTime;
                if (seconds >= 60f)
                {
                    minute++;
                    seconds = seconds - 60;
                }
                //�@�l���ς�����������e�L�X�gUI���X�V
                if ((int)seconds != (int)oldSeconds)
                {
                    timerText.text = minute.ToString("00") + ":" + ((int)seconds).ToString("00");
                }
                oldSeconds = seconds;

            }
            else
            {
                this.gameObject.SetActive(false);
                bStart = false;
            }
            SumTime = seconds + (minute * 60);
        }
    }

    public float GetTime()
    {
        return SumTime;
    }

    public void TimerStart()
    {
        // �^�C�}�[�J�n
        bStart = true;
        gameObject.GetComponent<Text>().enabled = bStart;
    }
}
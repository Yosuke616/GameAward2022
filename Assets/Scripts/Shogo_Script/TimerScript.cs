using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{

    [SerializeField]
    private int minute;
    [SerializeField]
    private float seconds;

    //　前のUpdateの時の秒数
    private float oldSeconds;
    //　タイマー表示用テキスト
    private Text timerText;
    // タイマー開始用フラグ
    private bool bStart;

    //合計の時間
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
                //　値が変わった時だけテキストUIを更新
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
        // タイマー開始
        bStart = true;
        gameObject.GetComponent<Text>().enabled = bStart;
    }
}
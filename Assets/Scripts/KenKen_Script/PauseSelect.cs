using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSelect : MonoBehaviour
{
    // ポーズパネル選択
    public GameObject PauseObj1;

    // ポーズパネル生成する場所
    public Transform parant1;


    // Start is called before the first frame update
    void Start()
    {
        // プレハブ生成
        PauseObj1 = Instantiate(PauseObj1, parant1);
        PauseObj1.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p") || Input.GetKeyDown("joystick button 7"))
        {
            //　ポーズUIのアクティブ、非アクティブを切り替え
            PauseObj1.SetActive(!PauseObj1.activeSelf);

            if (PauseObj1.activeSelf)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseContorol : MonoBehaviour
{
    public GameObject PauseObj;
    public Transform parant;

    // Start is called before the first frame update
    void Start()
    {
        PauseObj = Instantiate(PauseObj, parant);
        PauseObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p") || Input.GetKeyDown("joystick button 7"))
        {
            //　ポーズUIのアクティブ、非アクティブを切り替え
            PauseObj.SetActive(!PauseObj.activeSelf);

            //　ポーズUIが表示されてる時は停止
            if (PauseObj.activeSelf)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
}

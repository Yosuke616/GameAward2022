using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ポーズマネージャー用の追加
using UnityEngine.SceneManagement;

public class PauseContorol : MonoBehaviour
{
    // ポーズパネル選択
    public GameObject PauseObj;

    // ポーズパネル生成する場所
    public Transform parant;

    //止めるオブジェクト
    public GameObject gameobject;

    // Start is called before the first frame update
    void Start()
    {
        // プレハブ生成
        PauseObj = Instantiate(PauseObj, parant);
        PauseObj.SetActive(false);
        OnUnPause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p") || Input.GetKeyDown("joystick button 7"))
        {
            //　ポーズUIのアクティブ、非アクティブを切り替え
            PauseObj.SetActive(!PauseObj.activeSelf);
        }

        //　ポーズUIが表示されてる時は停止
        if (PauseObj.activeSelf)
        {
            OnPause();
        }
        else
        {
            OnUnPause();
        }

        Debug.Log(gameobject.transform.childCount);

    }

    //ポーズメニューに移行するための関数
    public void OnPause() {
        Time.timeScale = 0f;
        //子オブジェクトの全てのスクリプトを止める
        StopUpdate(gameobject.transform);
    }

    //ポーズメニューを解除するための関数
    public void OnUnPause() {
        Time.timeScale = 1f;
        //子オブジェクトの全てのスクリプトを再開
        StartUpdate(gameobject.transform);
    }

    //子オブジェクトのアップデートを止めるための関数
    void StopUpdate(Transform GO) {
       
        for (int i = 0; i < GO.childCount; i++)
        {
            Transform child = GO.GetChild(i);
            StopUpdate(child);
            foreach (MonoBehaviour mb in child.gameObject.GetComponents<MonoBehaviour>()) {
                mb.enabled = false;
            }
            
        }
    }

    //子オブジェクトのアップデートを再開する関数
    void StartUpdate(Transform GO) {
        for (int i = 0; i < GO.childCount; i++)
        {
            Transform child = GO.GetChild(i);
            StartUpdate(child);
            foreach (MonoBehaviour mb in child.gameObject.GetComponents<MonoBehaviour>())
            {
                mb.enabled = true;
            }
        }
    }
}

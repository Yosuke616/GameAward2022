using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink_Script : MonoBehaviour
{
    //プレイヤー用のリスト
    GameObject[] Players;

    //妖精用のリスト
    GameObject[] Fiaries;


    // Start is called before the first frame update
    void Start()
    {
        //プレイヤータグのついているオブジェクトを全て追加する
        Players = GameObject.FindGameObjectsWithTag("PlayerSub");

        //妖精タグのついているオブジェクトを全て追加する
        Fiaries = GameObject.FindGameObjectsWithTag("Fiary");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) {
            Debug.Log(Players.Length);
            Debug.Log(Fiaries.Length);
        }
    }


    //妖精とプレイヤーを点滅させる関数う
    public void Blink(bool blink)
    {
        if (blink)
        {
            //妖精とプレイヤーを消す
            foreach (GameObject P_Num in Players) {
                P_Num.gameObject.SetActive(false);
            }
            foreach (GameObject F_Num in Fiaries) {
                F_Num.gameObject.SetActive(false);
            }

        }
        else
        {
            //妖精とプレイヤーを出現させる
            foreach (GameObject P_Num in Players) {
                P_Num.gameObject.SetActive(true);
            }
            foreach (GameObject F_Num in Fiaries) {
                F_Num.gameObject.SetActive(true);
            }

        }
    }

    public void LastBlink() {
        //妖精とプレイヤーを消す
        foreach (GameObject P_Num in Players)
        {
            P_Num.gameObject.SetActive(false);
        }
        foreach (GameObject F_Num in Fiaries)
        {
            F_Num.gameObject.SetActive(false);
        }
    }
}

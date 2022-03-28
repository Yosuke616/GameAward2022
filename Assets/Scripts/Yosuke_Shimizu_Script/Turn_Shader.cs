using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn_Shader : MonoBehaviour { 

    //状態を保存する
    private int Mat_Sta;

    //マテリアルの得る為の変数
    Material mat;

    //Flipの数値をいじるための変数
    private float Flip_Num;

    // Start is called before the first frame update
    void Start()
    {
        //このコンポーネントがついたオフジェクトのマテリアルの情報を得る
        mat = this.GetComponent<MeshRenderer>().material;
        //ステータスを最初は何もしない
        Mat_Sta = 0;
        //最初は１でちゃんと見える
        Flip_Num = 1;
    }

    // Update is called once per frame
    void Update()
    {
        switch (Mat_Sta) {
            case 0:         //なにもしない
                break;
            case 1:         //消えてなくなる
                mat.SetFloat("_Flip",Flip_Num);
                if (Flip_Num >= -1) {
                    Flip_Num -= 0.05f;
                }
                break;
            case 2:         //現れる
                mat.SetFloat("_Flip", Flip_Num);
                if (Flip_Num  <= 1)
                {
                    Flip_Num += 0.05f;
                }
                break;
            default:break;
        }
    }

    //ボタンを押したときにめくれるような関数を作る(Setter)
    public void SetPaperSta(int Sta) {
        Mat_Sta = Sta;
    }
}

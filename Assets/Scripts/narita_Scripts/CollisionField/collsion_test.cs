using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collsion_test : MonoBehaviour
{
    // カメラに映っているあたり判定のもととなるオブジェクト
    private GameObject originalObject = null;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "paper")
        {
            return;
        }

        if (collider.gameObject.tag != "none")
        {
            //--- カメラに映っているオブジェクトをあたり判定フィールドに反映させるため情報を抜き取る
            // タグ
            this.tag = collider.gameObject.tag;
            // 傾き
            transform.Rotate(collider.gameObject.transform.localEulerAngles);
            // カメラに映っているオブジェクト
            originalObject = collider.gameObject;

            //エネミーの場合はエネミーの機能を追加する
            if (collider.gameObject.tag == "enemy")
            {
                //if(collider.gameObject.GetComponent<Enemy>() == null)
                //    collider.gameObject.AddComponent<Enemy>();
            }
            else if (collider.gameObject.tag == "CardSoldier")
            {
                if (collider.gameObject.GetComponent<EnemyBehavior>() == null)
                    collider.gameObject.AddComponent<EnemyBehavior>();
            }
        }
    }

    // カメラに映っているあたり判定のもととなるオブジェクト
    public GameObject getOriginalObject()
    {
        return originalObject;
    }

}

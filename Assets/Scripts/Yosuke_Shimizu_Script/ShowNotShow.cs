/*
 見えるか見えないかを決めるためのスクリプト
 これがあることによって

 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowNotShow : MonoBehaviour
{
    //試し用
    [SerializeField] Text text;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //見えている判定
    private void OnBecameVisible()
    {
        text.text = "見えている";
    }

    //見えていない判定
    private void OnBecameInvisible()
    {
        text.text = "見えていない";
    }
}

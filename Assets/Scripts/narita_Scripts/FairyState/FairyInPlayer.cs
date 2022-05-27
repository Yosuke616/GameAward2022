using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyInPlayer : MonoBehaviour
{
    // 紙の中にいる方の妖精のアニメーション

    private Animator animator;


    void Start()
    {

        animator = GetComponent<Animator>();


        if (!animator) Debug.LogError("アニメーターを取得できませんでした");
    }

    void Update()
    {
        
    }
}

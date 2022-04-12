/*
作成日：2022/3/14 Shimizu Shogo
内容  ：ボタンを押したときにSEを再生
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SETest : MonoBehaviour
{
    AudioClip clip;

    void Start()
    {
        clip = gameObject.GetComponent<AudioSource>().clip;
    }

    public void PlayStart()
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
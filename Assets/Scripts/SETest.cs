/*
�쐬���F2022/3/14 Shimizu Shogo
���e  �F�{�^�����������Ƃ���SE���Đ�
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
/*
�쐬���F2022/3/16 Shimizu Shogo
���e  �FBGM�Đ��p(�e�X�g)
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBGM : MonoBehaviour
{
    [SerializeField]
	//public string name_BGM;
    public AudioClip name_BGM;

	// Start is called before the first frame update
	void Start()
    {
		//SoundManager.Instance.PlayBgmByName(name_BGM);
		SoundManager.Instance.PlayBgmByName(name_BGM.name);
    }


    // Update is called once per frame
    void Update()
    {

    }
}

/*
 �����邩�����Ȃ��������߂邽�߂̃X�N���v�g
 ���ꂪ���邱�Ƃɂ����

 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowNotShow : MonoBehaviour
{
    //�����p
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

    //�����Ă��锻��
    private void OnBecameVisible()
    {
        text.text = "�����Ă���";
    }

    //�����Ă��Ȃ�����
    private void OnBecameInvisible()
    {
        text.text = "�����Ă��Ȃ�";
    }
}

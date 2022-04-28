using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public Material[] _mats = new Material[3];

    // Resorce.Load�Œ��ړǂݍ��݂̕��@���킩��Ȃ������̂ł����Ń}�e���A�����w��ł���悤�ɂ���
    public Material[] partitionMaterial = new Material[1];
    public Material GetPartitionMaterial()
    {
        return partitionMaterial[0];
    }

    enum GameState
    {
        MODE_NONE,          // ����ł��Ȃ�
        MODE_NOT_DIVIDE,    // �j���ԂłȂ�(�d�����v���C���[����ɂ���)
        MODE_DIVIDE,        // �j�鏈�����s��(�d���������Ă���)

    }


    private void Awake()
    {
        Application.targetFrameRate = 60;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looks : MonoBehaviour
{
    public Material[] mats = new Material[1];
    protected int frameCnt = 0;
    protected int createFrame = 60;
    [SerializeField] protected Vector3 CreateSpace;

    protected List<GameObject> looksObjects = new List<GameObject>();
    public void AddLooksObject(GameObject gameObject) { looksObjects.Add(gameObject); }

    void Start()
    {
        // ����������
        CreateSpace = new Vector3(45.0f, 25.0f, 10.0f);
    }

    void Update()
    {
        
    }


    // ���h���X�V
    public virtual void Updatelooks(){}

    // �I�u�W�F�N�g�̐���
    public virtual void Createlooks(){}
}

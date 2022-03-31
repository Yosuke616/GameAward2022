using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public Material[] _mats = new Material[3];

    private void Awake()
    {
        Application.targetFrameRate = 60;

        //_mats[0] = (Material)Resources.Load("Effects/Alpha");
        //_mats[1] = (Material)Resources.Load("Effects/Alpha_002");
        //_mats[2] = (Material)Resources.Load("Effects/Alpha_003");
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

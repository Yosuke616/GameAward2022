using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public Material[] _mats = new Material[3];

    // Resorce.Loadで直接読み込みの方法がわからなかったのでここでマテリアルを指定できるようにする
    public Material[] partitionMaterial = new Material[1];
    public Material GetPartitionMaterial()
    {
        return partitionMaterial[0];
    }

    enum GameState
    {
        MODE_NONE,          // 操作できない
        MODE_NOT_DIVIDE,    // 破る状態でない(妖精がプレイヤー周りにいる)
        MODE_DIVIDE,        // 破る処理実行中(妖精が動いている)

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooksManager : MonoBehaviour
{
    public Looks looks;

    private List<GameObject> looksObjects = new List<GameObject>();
    public void AddLooksObject(GameObject gameObject) { looksObjects.Add(gameObject); }

    void Start()
    {
    }

    void Update()
    {
        // めくるモードの時は更新しない
        if (CursorSystem.GetGameState() == CursorSystem.GameState.MODE_TURN_PAGES) return;

        if (looks)
        {
            // オブジェクト生成
            looks.Createlooks();

            // 動き更新
            looks.Updatelooks();
        }
    }
}

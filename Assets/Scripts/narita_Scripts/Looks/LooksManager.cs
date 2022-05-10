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
        // �߂��郂�[�h�̎��͍X�V���Ȃ�
        if (CursorSystem.GetGameState() == CursorSystem.GameState.MODE_TURN_PAGES) return;

        if (looks)
        {
            // �I�u�W�F�N�g����
            looks.Createlooks();

            // �����X�V
            looks.Updatelooks();
        }
    }
}

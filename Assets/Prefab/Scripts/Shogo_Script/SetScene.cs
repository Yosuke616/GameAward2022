using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetScene : MonoBehaviour
{
    [SerializeField]
    public SceneObject _setScene;

    public void MoveScene()
    {
        FadeManager.Instance.FadeStart(_setScene);
    }
}

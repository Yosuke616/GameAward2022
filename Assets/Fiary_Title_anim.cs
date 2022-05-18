using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fiary_Title_anim : MonoBehaviour
{
    private ModelAnimation animation;

    // Start is called before the first frame update
    void Start()
    {
        animation = GetComponent<ModelAnimation>();
        animation.SetAnim("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

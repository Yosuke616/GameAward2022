using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TestEffect : MonoBehaviour
{
    public VisualEffect effect;

    // Start is called before the first frame update
    void Start()
    {
        effect = Instantiate(effect);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlay()
    {
        effect.SendEvent("OnPlay");
    }

    public void StopPlay()
    {
        effect.SendEvent("StopPlay");
    }
}

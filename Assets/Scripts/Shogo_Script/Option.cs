using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public Slider FirstSelectSlider;

    // Start is called before the first frame update
    void Start()
    {
        FirstSelectSlider.Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

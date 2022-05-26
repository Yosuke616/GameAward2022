// Image点滅表現スクリプト
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageBlinking : MonoBehaviour
{
	public float fVol;

	private bool bUp;
	private Color clr;
    // Start is called before the first frame update
    void Start()
    {
		fVol = 0.025f;
		bUp = false;
		clr = GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
		if(bUp)
		{
			clr = new Color(clr.r, clr.g, clr.b, clr.a + fVol);
			GetComponent<Image>().color = clr;
			if (clr.a >= 1.0f)
				bUp = false;
		}
		else
		{
			clr = new Color(clr.r, clr.g, clr.b, clr.a - fVol);
			GetComponent<Image>().color = clr;
			if (clr.a <= 0.0f)
				bUp = true;
		}
    }
}

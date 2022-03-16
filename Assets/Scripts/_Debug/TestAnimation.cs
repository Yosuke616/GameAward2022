using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimation : MonoBehaviour
{
	Animator anim;
	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void OnGUI()
	{
		if (GUI.Button(new Rect(20, 10, 80, 20), "Walk"))
		{
			anim.CrossFade("Walk", 0);
		}
		if (GUI.Button(new Rect(20, 30, 80, 20), "Action"))
		{
			anim.CrossFade("Action", 0);
		}
		if (GUI.Button(new Rect(20, 50, 80, 20), "Stand-by"))
		{
			anim.CrossFade("Stand-by", 0);
		}
	}
}

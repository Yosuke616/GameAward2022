using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class TestAnimation : MonoBehaviour
{
	Animator anim;
	AnimatorController controller;

	void Awake()
	{
		anim = GetComponent<Animator>();

		var runtimeController = anim.runtimeAnimatorController;

		// UnityEditor‚ÌAnimatorController‚ÅƒLƒƒƒXƒg‚·‚é .
		controller = runtimeController as UnityEditor.Animations.AnimatorController;
	}

	void OnGUI()
	{
		foreach (var layer in controller.layers)
		{
			UnityEditor.Animations.AnimatorStateMachine stateMachine = layer.stateMachine;

			int i = 0;
			foreach (var state in stateMachine.states)
			{
				if (GUI.Button(new Rect(20, 10 + 20 * i, 80, 20), state.state.name))
				{
					anim.CrossFade(state.state.name, 0);
				}
				i++;
			}
		}

		//if (GUI.Button(new Rect(20, 10, 80, 20), "Walk"))
		//{
		//	anim.CrossFade("Walk", 0);
		//}
		//if (GUI.Button(new Rect(20, 30, 80, 20), "Action"))
		//{
		//	anim.CrossFade("Action", 0);
		//}
		//if (GUI.Button(new Rect(20, 50, 80, 20), "Stand-by"))
		//{
		//	anim.CrossFade("Stand-by", 0);
		//}
	}
}
